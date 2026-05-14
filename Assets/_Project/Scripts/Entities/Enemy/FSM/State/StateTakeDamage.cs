using System;
using System.Threading;
using _Project.Scripts.Enemy.FSM.Core;
using _Project.Scripts.Enemy.FSM.State.Component;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemy.FSM.State
{
    public class StateTakeDamage : IState
    {
        private StateMachineEnemy _stateMachineEnemy;
        private EnemyAnimationService _enemyAnimationService;

        private float _contusionTime;

        private NavMeshAgent _agent;
        private CancellationToken _token;

        public StateTakeDamage(EnemyAnimationService enemyAnimationService, StateMachineEnemy stateMachineEnemy,
            NavMeshAgent agent,
            CancellationToken token,
            float contusionTime)
        {
            _stateMachineEnemy = stateMachineEnemy;
            _enemyAnimationService = enemyAnimationService;

            _agent = agent;
            _token = token;
            _contusionTime = contusionTime;
        }

        public StateId StateId => StateId.TakeDamage;

        public void Enter()
        {
            Debug.Log("В получение урона");
            _agent.isStopped = true;
        }

        public void Update()
        {
            DelayChasing(_token).Forget();
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
            Debug.Log("Вышли получение урона");
        }

        private async UniTask DelayChasing(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_contusionTime), cancellationToken: token);

            _agent.isStopped = false;
            _stateMachineEnemy.SwitchState(StateId.Chasing);
        }
    }
}