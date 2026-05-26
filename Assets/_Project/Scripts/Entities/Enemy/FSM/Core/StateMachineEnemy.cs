using System.Collections.Generic;
using _Project.Scripts.Enemy.FSM.State.Component;
using _Project.Scripts.Entities.Enemy.FSM.Core;

namespace _Project.Scripts.Enemy.FSM
{
    public class StateMachineEnemy
    {
        private readonly Dictionary<StateId, IState> _states = new();

        private IState _currentState;
        private EnemyAnimationService _enemyAnimationService;

        public StateMachineEnemy(EnemyAnimationService enemyAnimationService)
        {
            _enemyAnimationService = enemyAnimationService;
        }

        public void Register(IState state)
        {
            _states[state.StateId] = state;
        }

        public void SwitchState(StateId id)
        {
            if (_currentState?.StateId == id)
                return;

            _currentState?.Exit();

            if (_states.TryGetValue(id, out var state))
            {
                _currentState = state;
                _enemyAnimationService.SetAnimation(id);
                _currentState.Enter();
            }
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }
    }
}