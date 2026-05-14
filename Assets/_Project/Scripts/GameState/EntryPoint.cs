using _Project.Scripts.Entities.Enemy;
using _Project.Scripts.UI;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameState
{
    public class EntryPoint : BaseState
    {
        [SerializeField] private Transform _spawnPointPlayer;
        [SerializeField] private CinemachineVirtualCamera _camera;

        private void Awake()
        {
            ExecuteStartGame().Forget();
        }

        private async UniTask ExecuteStartGame()
        {
            FadeImage.FadeOut().Forget();

            Player.enabled = true;
            Player.Resurrection();
            Player.transform.position = _spawnPointPlayer.position;

            EnemySpawner.Initialize();
            EnemySpawner.Enable();

            _camera.enabled = true;
            _camera.Follow = Player.transform;
            _camera.LookAt = Player.transform;
        }
    }
}