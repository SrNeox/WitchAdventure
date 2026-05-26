using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.CameraPlayer
{
    public class CameraPlayer : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _playerCamera;

        private void Start()
        {
            _playerCamera.transform.DOShakePosition(1, 2f);
        }
    }
}