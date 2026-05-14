using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Scripts.Input
{
    public class InputService : IInitializable, IDisposable
    {
        private readonly PlayerInputSystem _playerInput = new PlayerInputSystem();

        public Vector2 Direction { get; private set; }

        public event Action IsAttacking;

        public void Initialize()
        {
            _playerInput.Enable();

            _playerInput.Player.Move.performed += OnMove;
            _playerInput.Player.Move.canceled += OnMove;

            _playerInput.Player.Attack.performed += OnAttack;
        }

        public void Dispose()
        {
            _playerInput.Disable();

            _playerInput.Player.Move.performed -= OnMove;
            _playerInput.Player.Move.canceled -= OnMove;

            _playerInput.Player.Attack.performed -= OnAttack;
        }

        public Vector3 GetMosePosition()
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            return mousePos;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Direction = context.ReadValue<Vector2>();
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
                IsAttacking?.Invoke();
        }
    }
}