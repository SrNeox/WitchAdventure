using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.AudioData
{
    public class AudioData : IInitializable, IDisposable
    {
        private readonly Dictionary<AudioListName, AudioClip> _audioClips = new();
        private readonly CancellationTokenSource _cts = new();

        public AudioClip GetAudioClip(AudioListName audioListName)
        {
            return _audioClips.GetValueOrDefault(audioListName);
        }

        private async UniTask LoadResources()
        {
            var audioClipEnterTask = Addressables
                .LoadAssetAsync<AudioClip>(AudioListName.AudioClipPointerEnter.ToString())
                .ToUniTask(cancellationToken: _cts.Token);

            var audioClipDownTask = Addressables
                .LoadAssetAsync<AudioClip>(AudioListName.AudioClipPointerDown.ToString())
                .ToUniTask(cancellationToken: _cts.Token);

            var (audioClipEnter, audioClipDown) = await UniTask.WhenAll(audioClipEnterTask, audioClipDownTask);

            _audioClips.Add(AudioListName.AudioClipPointerEnter, audioClipEnter);
            _audioClips.Add(AudioListName.AudioClipPointerDown, audioClipDown);
        }

        public void Initialize()
        {
            LoadResources().Forget();
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();

            foreach (var clip in _audioClips.Values)
            {
                if (clip != null)
                {
                    Addressables.Release(clip);
                }
            }

            _audioClips.Clear();
        }
    }
}