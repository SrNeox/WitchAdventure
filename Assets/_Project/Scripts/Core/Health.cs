using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts
{
    public class Health
    {
        private int _health;
        private int _maxHealth;
        private float _timeDelayDamage;
        private bool _canTakeDamage = true;
        
        private readonly CancellationToken _cancellationToken;

        public Health(int maxHealth, float timeDelayDamage, CancellationToken token)
        {
            _maxHealth = maxHealth;
            _health = _maxHealth;
            _timeDelayDamage = timeDelayDamage;
            _cancellationToken = token;
        }

        public int CurrentHealth => _health;

        public event Action OnDeath;
        public event Action OnTakeDamage;

        public void TakeDamage(int damage)
        {
            if (_canTakeDamage == false || _health <= 0)
                return;

            _health -= damage;
            OnTakeDamage?.Invoke();

            if (_health <= 0)
                OnDeath?.Invoke();

            _canTakeDamage = false;
            DelayTakeDamage(_timeDelayDamage, _cancellationToken).Forget();
        }

        private async UniTask DelayTakeDamage(float timeDelayDamage, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeDelayDamage), cancellationToken: token);

            _canTakeDamage = true;
        }

        public void Healing(int healthAdd)
        {
            if (_health > _maxHealth)
                return;

            _health += healthAdd;

            if (_health > _maxHealth)
                _health = _maxHealth;
        }

        public void ResetHealth()
        {
            _health = _maxHealth;
            _canTakeDamage = true;
        }
    }
}