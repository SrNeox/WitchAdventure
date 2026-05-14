using UnityEngine;

namespace _Project.Scripts
{
    public interface IDamageable 
    {
        void TakeDamage(int damage , Transform enemyPosition );
    }
}
