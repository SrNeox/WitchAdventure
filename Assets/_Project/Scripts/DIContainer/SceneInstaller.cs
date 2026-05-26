using UnityEngine;
using Zenject;

namespace _Project.Scripts.DIContainer
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Entities.Enemy.EnemySpawner _spawner;
        [SerializeField] private AudioSource _audioSourceUI;

        public override void InstallBindings()
        {
            Container.Bind<Entities.Enemy.EnemySpawner>().FromInstance(_spawner).AsSingle();
            Container.BindInstance(_audioSourceUI).AsSingle();
            Container.BindInterfacesAndSelfTo<AudioData.AudioData>().AsSingle().NonLazy();
        }
    }
}