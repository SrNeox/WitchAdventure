using System;
using UnityEngine;

namespace _Project.Scripts.Enemy.FSM
{
    public class SearcherPlayer : MonoBehaviour
    {
        public event Action<Transform> OnFoundPlayer;
        public event Action OnMissPlayer;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Entities.Player.Player player))
            {
                OnFoundPlayer?.Invoke(player.transform);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Entities.Player.Player player))
            {
                OnMissPlayer?.Invoke();
            }
        }
    }
}