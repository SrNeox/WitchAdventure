using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Entities.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyAI _enemyPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private int _startPoolCount = 10;
        [SerializeField] private int _delayNextWaveSeconds = 10;

        private ObjectPool<EnemyAI> _pool;

        private bool _isWorking;
        private CancellationTokenSource _token;

        public void Initialize()
        {
            _pool = new ObjectPool<EnemyAI>(_enemyPrefab, _startPoolCount, transform);
        }

        public void Enable()
        {
            _token = new CancellationTokenSource();
            _token = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, _token.Token);

            _isWorking = true;
            Spawn().Forget();
        }

        public void Disable()
        {
            _isWorking = false;
            _token.Cancel();
            _token.Dispose();
            _token = null;
        }

        public async UniTask Spawn()
        {
            while (_token.IsCancellationRequested == false && _isWorking)
            {
                for (int i = 0; i < _spawnPoints.Length; i++)
                {
                    EnemyAI enemy = _pool.Get();

                    enemy.transform.position = _spawnPoints[i].position;
                    enemy.transform.rotation = Quaternion.identity;

                    enemy.OnDeath += Despawn;

                    enemy.Revive();
                }

                await UniTask.Delay(TimeSpan.FromSeconds(_delayNextWaveSeconds), cancellationToken: _token.Token);
            }
        }

        private void Despawn(EnemyAI enemy)
        {
            enemy.OnDeath -= Despawn;

            _pool.Return(enemy);
        }
    }
}