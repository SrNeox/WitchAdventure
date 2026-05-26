using _Project.Scripts.Enemy.FSM.State.Component;
using _Project.Scripts.Entities.Enemy.FSM.Core;
using UnityEngine;

namespace _Project.Scripts.Enemy.FSM.State
{
    public class StateDie : IState
    {
        private readonly EnemyAnimationService _enemyAnimationService;

        private Collider2D _collider;

        public StateDie(EnemyAnimationService enemyAnimationService, Collider2D collider2D)
        {
            _collider = collider2D;
            _enemyAnimationService = enemyAnimationService;
        }

        public StateId StateId => StateId.Die;

        public void Enter()
        {
            _collider.enabled = false;
            _enemyAnimationService.SetAnimation(StateId);
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
        }
    }
}