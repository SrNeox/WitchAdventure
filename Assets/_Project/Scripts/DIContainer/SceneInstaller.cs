using UnityEngine;
using Zenject;

namespace _Project.Scripts.DIContainer
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Entities.Enemy.EnemySpawner _spawner;

        public override void InstallBindings()
        {
            Container.Bind<Entities.Enemy.EnemySpawner>().FromInstance(_spawner).AsSingle();
        }
    }
}