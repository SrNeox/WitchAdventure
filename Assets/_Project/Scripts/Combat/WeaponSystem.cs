using System;
using _Project.Scripts.Combat;
using _Project.Scripts.Input;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] private Sword _sword;
        

        private InputService _inputService;

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
        }

        private void OnEnable()
        {
            _inputService.IsAttacking += _sword.Attack;
        }

        private void OnDisable()
        {
            _inputService.IsAttacking -= _sword.Attack;
        }
    }
}