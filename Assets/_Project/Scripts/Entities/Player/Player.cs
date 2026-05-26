using System;
using System.Threading;
using _Project.Scripts.Core;
using _Project.Scripts.Entities.Player.AnimationService;
using _Project.Scripts.Input;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Entities.Player
{
    [SelectionBase]
    public class Player : MonoBehaviour, IDamageable
    {
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _forceDash = 20;
        [SerializeField] private float _delayForce = 0.1f;

        [Header("AnimatorEffects")] [SerializeField]
        private Animator _animatorEffects;

        private Animator _animator;
        private Rigidbody2D _rigidbody;

        private Health _health;
        private InputService _inputService;
        private PlayerAnimationService _playerAnimationService;

        private bool _isDead;
        private bool _isKnocked;
        private float _knockTimer;

        public event Action IsDeath;

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
        }

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            _health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            _health.OnDeath -= OnDeath;
        }

        private void FixedUpdate()
        {
            if (_isKnocked || _isDead)
                return;

            Move();
        }

        private void Update()
        {
            if (_isKnocked || _isDead)
                return;

            Rotate();
        }

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _health = new Health(_maxHealth, _knockTimer, this.destroyCancellationToken);
            _playerAnimationService = new PlayerAnimationService(_animator , _animatorEffects);
        }

        public void TakeDamage(int damage, Transform enemyPosition)
        {
            _health.TakeDamage(damage);
            _playerAnimationService.TriggerTakeDamage();

            var direction = (transform.position - enemyPosition.position).normalized;
            PushAway(direction, this.destroyCancellationToken).Forget();

            Debug.Log(_health.CurrentHealth);
        }

        public async UniTask PushAway(Vector2 direction, CancellationToken token)
        {
            if (_isKnocked)
                return;

            _isKnocked = true;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(direction * _forceDash, ForceMode2D.Impulse);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayForce), cancellationToken: token);

            _rigidbody.velocity = Vector2.zero;
            _isKnocked = false;
        }

        public void Resurrection()
        {
            foreach (var child in GetComponentsInChildren<Transform>(true))
            {
                if (child.transform == transform)
                    continue;

                child.gameObject.SetActive(true);
            }

            _isDead = false;
            _playerAnimationService.TriggerResurrection();
        }

        private void Move()
        {
            float velocity = _inputService.Direction.sqrMagnitude;
            _playerAnimationService.SetSpeed(velocity);
            _rigidbody.velocity = _inputService.Direction * _moveSpeed;
        }

        private void Rotate()
        {
            float dir = Mathf.Sign(_inputService.GetMosePosition().x - GetPlayerScreenPosition().x);

            Vector3 scale = transform.localScale;
            scale.x = dir;
            transform.localScale = scale;
        }

        private Vector3 GetPlayerScreenPosition()
        {
            Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
            return position;
        }

        private void OnDeath()
        {
            foreach (var child in GetComponentsInChildren<Transform>(true))
            {
                if (child.transform == transform)
                    continue;

                child.gameObject.SetActive(false);
            }

            _isDead = true;
            _rigidbody.bodyType = RigidbodyType2D.Static;
            _playerAnimationService.TriggerDeath();
            IsDeath?.Invoke();
        }
    }
}