namespace _Project.Scripts.Enemy.FSM.State.Component
{
    public interface IState
    {
        StateId StateId { get; }

        public void Enter();
        public void Update();
        public void FixedUpdate();
        public void Exit();
    }
}