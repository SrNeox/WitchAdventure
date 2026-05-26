using _Project.Scripts.Enemy.FSM.State.Component;
using _Project.Scripts.Entities.Enemy.FSM.Core;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemy.FSM.State
{
    public class StateChasing : IState
    {
        private static readonly int Speed = Animator.StringToHash("Speed");

        private NavMeshAgent _agent;
        private StateMachineEnemy _stateMachineEnemy;
        private EnemyAnimationService _enemyAnimationService;

        private Transform _target;
        private Transform _transform;

        public StateChasing(NavMeshAgent agent, Transform enemyTransform, EnemyAnimationService enemyAnimationService,
            StateMachineEnemy stateMachineEnemy)
        {
            _agent = agent;
            _transform = enemyTransform;
            _enemyAnimationService = enemyAnimationService;
            _stateMachineEnemy = stateMachineEnemy;
        }

        public StateId StateId => StateId.Chasing;

        public void Enter()
        {
        }

        public void Update()
        {
            if (_target == null)
                return;

            float distance = Vector3.Distance(_transform.position, _target.position);

            if (_agent.stoppingDistance < distance)
            {
                Chase();
            }
            
            if (_agent.stoppingDistance >= distance)
            {
                _agent.ResetPath();
                _enemyAnimationService.SetSpeed(0f);
                _stateMachineEnemy.SwitchState(StateId.Attacking);
            }
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void Chase()
        {
            Rotate();
            _agent.SetDestination(_target.position);
            _enemyAnimationService.SetSpeed(_agent.velocity.sqrMagnitude);
        }


        private void Rotate()
        {
            float dir = Mathf.Sign(_agent.velocity.x);

            Vector3 scale = _transform.localScale;
            scale.x = dir;
            _transform.localScale = scale;
        }
    }
}