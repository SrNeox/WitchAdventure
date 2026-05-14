using _Project.Scripts.Entities.Enemy;
using _Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameState
{
    public class BaseState : MonoBehaviour
    {
        [SerializeField] private FadeImage _fadeImage;

        public FadeImage FadeImage => _fadeImage;
        public Player.Player Player { get; private set; }
        public EnemySpawner EnemySpawner { get; private set; }

        [Inject]
        private void Construct(Player.Player player, EnemySpawner spawner)
        {
            Player = player;
            EnemySpawner = spawner;
        }
    }
}