using UnityEngine;

namespace _Project.Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
    public class EnemySO : UnityEngine.ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        [field: SerializeField] public float TimeDelayTakingDamage { get; private set; } = 0.2f;
        [field: SerializeField] public float Contusion { get; private set; } = 0.5f;

        [field: SerializeField] public float PatrolTime { get; private set; } = 2f;
        [field: SerializeField] public float MaxDistancePatrol { get; private set; } = 7f;
        [field: SerializeField] public float MinDistancePatrol { get; private set; } = 3f;
        
        [field: SerializeField] public int Damage { get; private set; } = 5;
        [field: SerializeField] public float AttackCooldown { get; private set; } = 1f;
        [field: SerializeField] public Vector2 AttackBoxSize { get; private set; } = new Vector2(4, 1f);
    }
}