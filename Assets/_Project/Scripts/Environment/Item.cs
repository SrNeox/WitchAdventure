using System;
using _Project.Scripts.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Environment
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private float _delayRestore = 10;

        private Health _health;
        private SpriteRenderer _spriteRenderer;
        private ParticleSystem _particleSystem;

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
        }

        private async UniTask Start()
        {
            _particleSystem.Play();
            gameObject.SetActive(false);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayRestore));
            
            
            
        }
    }
}