using System;
using System.Threading;
using _Project.Scripts.Enemy.FSM;
using _Project.Scripts.Enemy.FSM.Core;
using _Project.Scripts.Enemy.FSM.State;
using _Project.Scripts.Enemy.FSM.State.Component;
using _Project.Scripts.ScriptableObject;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Entities.Enemy
{
    public class EnemyAI : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemySO _config; 
        
        [SerializeField] private Transform _attackPoint; 

        private Health _health;
        private Animator _animator;
        private NavMeshAgent _agent;
        private Collider2D _colliderBody;
        private SearcherPlayer _searcherPlayer;

        private Vector3 _startingPosition;

        private StateMachineEnemy _stateMachineEnemy;
        private EnemyAnimationService _enemyAnimationService;

        private StateIdle _stateIdle;
        private StatePatrol _statePatrol;
        private StateChasing _stateChasing;
        private StateAttacking _stateAttacking;
        private StateTakeDamage _stateTakeDamage;
        private StateDie _stateDie;

        public event Action<EnemyAI> OnDeath;

        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            _stateMachineEnemy.FixedUpdate();
        }

        private void Update()
        {
            _stateMachineEnemy.Update();
        }

        private void OnEnable()
        {
            _searcherPlayer.OnFoundPlayer += ChasingTarget;
            _searcherPlayer.OnMissPlayer += StopChasing;

            _health.OnDeath += Die;
        }

        private void OnDisable()
        {
            _searcherPlayer.OnFoundPlayer -= ChasingTarget;
            _searcherPlayer.OnMissPlayer -= StopChasing;

            _health.OnDeath -= Die;
        }

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _colliderBody = GetComponent<Collider2D>();
            _searcherPlayer = GetComponentInChildren<SearcherPlayer>();

            _agent.updateUpAxis = false;
            _agent.updateRotation = false;
            
            _health = new Health(_config.MaxHealth, _config.TimeDelayTakingDamage, this.destroyCancellationToken);
            _enemyAnimationService = new EnemyAnimationService(_animator);
            _stateMachineEnemy = new StateMachineEnemy(_enemyAnimationService);

            RegisterStates();
        }

        private void RegisterStates()
        {
            _stateIdle = new StateIdle();
            
            _statePatrol = new StatePatrol(
                _agent, transform, _enemyAnimationService,
                _config.MaxDistancePatrol, _config.MinDistancePatrol, _config.PatrolTime);

            _stateChasing = new StateChasing(
                _agent, transform, _enemyAnimationService, _stateMachineEnemy);

            _stateDie = new StateDie(_enemyAnimationService, _colliderBody);
            
            _stateAttacking = new StateAttacking(
                _enemyAnimationService, _stateMachineEnemy,
                _config.AttackBoxSize, _attackPoint, _config.Damage, _config.AttackCooldown);
            
            _stateTakeDamage = new StateTakeDamage(
                _enemyAnimationService, _stateMachineEnemy,
                _agent, this.destroyCancellationToken, _config.Contusion);

            _stateMachineEnemy.Register(_stateIdle);
            _stateMachineEnemy.Register(_statePatrol);
            _stateMachineEnemy.Register(_stateChasing);
            _stateMachineEnemy.Register(_stateAttacking);
            _stateMachineEnemy.Register(_stateTakeDamage);
            _stateMachineEnemy.Register(_stateDie);

            _stateMachineEnemy.SwitchState(StateId.Patrol);
        }

        private void ChasingTarget(Transform target)
        {
            _stateChasing.SetTarget(target);
            _stateAttacking.SetTarget(target, transform);
            _stateMachineEnemy.SwitchState(StateId.Chasing);
        }

        private void StopChasing()
        {
            Patrol(this.destroyCancellationToken).Forget();
            _stateChasing.SetTarget(null);
        }

        private async UniTask Patrol(CancellationToken token)
        {
            _stateMachineEnemy.SwitchState(StateId.Idle);

            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: token);

            _stateMachineEnemy.SwitchState(StateId.Patrol);
        }

        private void Die()
        {
            _stateMachineEnemy.SwitchState(StateId.Die);
            this.enabled = false;

            AnimationAfterDeath(this.destroyCancellationToken).Forget();
        }

        private async UniTask AnimationAfterDeath(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: token);

            await transform.DOScale(Vector3.zero, 1)
                .SetEase(Ease.InElastic)
                .From(transform.localScale + Vector3.one)
                .WithCancellation(token);

            OnDeath?.Invoke(this);
        }

        public void Revive()
        {
            _colliderBody.enabled = true;
            _health.ResetHealth();
            _stateMachineEnemy.SwitchState(StateId.Patrol);
            this.enabled = true;
            transform.localScale = Vector3.one;
        }

        public void TakeDamage(int damage, Transform enemyPosition)
        {
            _health.TakeDamage(damage);

            if (_health.CurrentHealth > 0)
            {
                _stateMachineEnemy.SwitchState(StateId.TakeDamage);
            }
        }

        public void OnDrawGizmosSelected()
        {
            if (transform == null || _config == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_attackPoint.position, _config.AttackBoxSize);
        }
    }
}