using _Project.Scripts.Core;
using _Project.Scripts.Enemy.FSM.State.Component;
using UnityEngine;

namespace _Project.Scripts.Enemy.FSM.Core
{
    public class EnemyAnimationService : BaseAnimationService
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        public EnemyAnimationService(Animator animator) : base(animator)
        {
        }

        public void SetAnimation(StateId stateId)
        {
            ResetAnimation();

            switch (stateId)
            {
                case StateId.Idle:
                    Animator.SetBool(Idle, true); 
                    break;
                case StateId.Attacking:
                    Animator.SetTrigger(Attacking);
                    break;
                case StateId.TakeDamage:
                    TriggerTakeDamage(); 
                    break;
                case StateId.Die:
                    TriggerDeath();
                    break;
            }
        }

        private void ResetAnimation()
        {
            Animator.SetBool(Idle, false);
            SetSpeed(0);
        }
    }
}