using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly Queue<T> _pool = new Queue<T>();
        private readonly T _prefab;
        private readonly Transform _parent;

        public ObjectPool(T prefab, int initialCount, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;

            CreateStartCount(initialCount);
        }

        private void CreateStartCount(int initialCount)
        {
            for (int i = 0; i < initialCount; i++)
            {
                T obj = Object.Instantiate(_prefab, _parent);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public T Get()
        {
            T obj;

            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
            }
            else
            {
                obj = Object.Instantiate(_prefab, _parent);
            }
            
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Return(T obj)
        {
            if (_pool.Contains(obj))
            {
                Debug.LogWarning($"[ObjectPool] Попытка вернуть в пул объект {obj.name}, который уже находится там!");
                return;
            }

            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}