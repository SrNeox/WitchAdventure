using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class FadeImage : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _duration;

        public async UniTask FadeIn()
        {
            await _image.DOFade(1, _duration).SetEase(Ease.Linear)
                .WithCancellation(Application.exitCancellationToken).SuppressCancellationThrow();
        }

        public async UniTask FadeOut()
        {
            await _image.DOFade(0, _duration).SetEase(Ease.Linear)
                .WithCancellation(Application.exitCancellationToken).SuppressCancellationThrow();
        }
    }
}