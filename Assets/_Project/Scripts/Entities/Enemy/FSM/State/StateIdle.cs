using _Project.Scripts.Enemy.FSM.State.Component;

namespace _Project.Scripts.Enemy.FSM.State
{
    public class StateIdle : IState
    {
        public StateId StateId => StateId.Idle;

        public void Enter() { }

        public void Update() { }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}