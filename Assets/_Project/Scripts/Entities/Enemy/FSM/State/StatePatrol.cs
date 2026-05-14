using _Project.Scripts.Enemy.FSM.Core;
using _Project.Scripts.Enemy.FSM.State.Component;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Enemy.FSM.State
{
    public class StatePatrol : IState
    {
        public StateId StateId => StateId.Patrol;

        private readonly NavMeshAgent _agent;
        private readonly EnemyAnimationService _enemyAnimationService;
        
        private readonly float _delayNextPoint;      
        
        private readonly float _maxDistancePatrol;   
        private readonly float _minDistancePatrol;   

        private Vector3 _startPoint;
        private Vector3 _patrolPoint;
        private Transform _transform;
        
        private float _timer;
        
        public StatePatrol(
            NavMeshAgent agent,
            Transform enemyPosition,
            EnemyAnimationService enemyAnimationService,
            float maxDistancePatrol,
            float minDistancePatrol,
            float delayNextPoint)
        {
            _agent = agent;
            _enemyAnimationService = enemyAnimationService;
            _transform = enemyPosition;

            _maxDistancePatrol = maxDistancePatrol;
            _minDistancePatrol = minDistancePatrol;
            _delayNextPoint = delayNextPoint;
        }

        public void Enter()
        {
            _timer = 0;
            _startPoint = _transform.position;
        }

        public void Update()
        {
            Patrol();
        }

        public void FixedUpdate() { }

        public void Exit() { }

        private void Patrol()
        {
            if (_timer <= 0)
            {
                _patrolPoint = GetPointPatrol();
                _timer = _delayNextPoint;
            }

            _timer -= Time.deltaTime;

            _agent.SetDestination(_patrolPoint);
            _enemyAnimationService.SetSpeed(_agent.velocity.sqrMagnitude);

            Rotate();
        }

        private Vector3 GetPointPatrol()
        {
            Vector3 dir = new(Random.Range(-1, 2), Random.Range(-1, 2));
            float dist = Random.Range(_minDistancePatrol, _maxDistancePatrol);
            return _startPoint + dir * dist;
        }

        private void Rotate()
        {
            float dir = Mathf.Sign(_agent.velocity.x);
            if (dir == 0) return;

            Vector3 scale = _transform.localScale;
            scale.x = dir;
            _transform.localScale = scale;
        }
    }
}
