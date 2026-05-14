using UnityEngine;

namespace _Project.Scripts.Core
{
    public abstract class BaseAnimationService
    {
        protected readonly Animator Animator; 
        
        private static readonly int DieHash = Animator.StringToHash("Die");
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int Resurrection = Animator.StringToHash("Resurrection");
        private static readonly int TakeDamageHash = Animator.StringToHash("TakeDamage");

        protected BaseAnimationService(Animator animator)
        {
            Animator = animator;
        }

        public void SetSpeed(float speed)
        {
            Animator.SetFloat(SpeedHash, speed);
        }

        public void TriggerTakeDamage()
        {
            Animator.SetTrigger(TakeDamageHash);
        }

        public void TriggerDeath()
        {
            Animator.SetTrigger(DieHash);
        }

        public void TriggerResurrection()
        {
            Animator.SetTrigger(Resurrection);
        }
    }
}
