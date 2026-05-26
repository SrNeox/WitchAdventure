using _Project.Scripts.Input;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.DIContainer
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private Entities.Player.Player _playerPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle().NonLazy();
            Container.Bind<Entities.Player.Player>().FromComponentInNewPrefab(_playerPrefab).AsSingle().NonLazy();
        }
    }
}