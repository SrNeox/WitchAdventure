using System;
using System.Threading;
using _Project.Scripts.Enemy.FSM.State.Component;
using _Project.Scripts.Entities.Enemy.FSM.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Enemy.FSM.State
{
    public class StateAttacking : IState
    {
        private readonly EnemyAnimationService _enemy;
        private readonly StateMachineEnemy _stateMachineEnemy;

        private CancellationTokenSource _tokenSource;

        private int _damage;
        private float _attackCooldown = 1f;

        private bool _isAttacking = false;

        private Transform _transformSelf;
        private Transform _transformTarget;

        private Vector2 _attackBoxSize;
        private Transform _attackPoint;
        private Collider2D[] _hits = new Collider2D[10];

        public StateAttacking(EnemyAnimationService enemyAnimationService, StateMachineEnemy stateMachineEnemy,
            Vector3 attackBoxSize,
            Transform attackPoint,
            int damage,
            float attackCooldown)
        {
            _enemy = enemyAnimationService;
            _stateMachineEnemy = stateMachineEnemy;

            _attackPoint = attackPoint;
            _attackBoxSize = attackBoxSize;

            _damage = damage;
            _attackCooldown = attackCooldown;
        }

        public StateId StateId => StateId.Attacking;

        public void Enter()
        {
            _tokenSource = new CancellationTokenSource();

            SetAttack(_tokenSource.Token).Forget();
        }

        public void Update()
        {
            if (_transformTarget == null)
            {
                _stateMachineEnemy.SwitchState(StateId.Patrol);
                return;
            }

            CheckDistance();

            if (!_isAttacking)
            {
                SetAttack(_tokenSource.Token).Forget();
            }
        }

        private void CheckDistance()
        {
            var distance = Vector3.Distance(_transformSelf.position, _transformTarget.position);

            if (distance > 1.5f && !_isAttacking)
            {
                _stateMachineEnemy.SwitchState(StateId.Chasing);
            }
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();
                _tokenSource.Dispose();
                _tokenSource = null;
            }
        }

        public void SetTarget(Transform target, Transform self)
        {
            _transformTarget = target;
            _transformSelf = self;
        }

        private async UniTask SetAttack(CancellationToken token)
        {
            _enemy.SetAnimation(StateId.Attacking);

            await AttackTarget(token);
        }

        private async UniTask AttackTarget(CancellationToken token)
        {
            Physics2D.OverlapBoxNonAlloc(_attackPoint.position, _attackBoxSize, 0f, _hits);

            foreach (Collider2D hit in _hits)
            {
                if (hit != null && hit.TryGetComponent(out IDamageable essence))
                {
                    essence.TakeDamage(_damage, _transformSelf);
                    _isAttacking = true;
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_attackCooldown), cancellationToken: token);

            _isAttacking = false;
        }
    }
}