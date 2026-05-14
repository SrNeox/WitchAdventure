using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class Sword : MonoBehaviour
    {
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");

        [SerializeField] private int _damage = 10;
        [SerializeField] private float _delayNextAttack = 1;

        [SerializeField] private Animator _animatorEffects;
        [SerializeField] private Animator _animatorSword;

        private bool _isAttack = true;

        private void Start()
        {
            _animatorSword = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage, transform);
            }
        }

        public void Attack()
        {
            if (_isAttack)
            {
                _isAttack = false;
                _animatorSword.SetTrigger(AttackAnimation);
                _animatorEffects.SetTrigger(AttackAnimation);
                DelayAttack(this.destroyCancellationToken).Forget();
            }
        }

        private async UniTask DelayAttack(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delayNextAttack));
            _isAttack = true;
        }
    }
}