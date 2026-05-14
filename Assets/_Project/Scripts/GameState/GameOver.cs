using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.GameState
{
    public class GameOver : BaseState
    {
        private void OnEnable()
        {
            Player.IsDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            Player.IsDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            SetState().Forget();
        }

        public async UniTask SetState()
        {
            EnemySpawner.Disable();
            await FadeImage.FadeIn();

            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
    }
}