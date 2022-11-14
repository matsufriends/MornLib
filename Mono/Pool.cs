using System.Collections.Generic;
using UnityEngine;
namespace MornLib.Mono {
    public class Pool<T> : MonoBehaviour,IPool<T> where T : MonoBehaviour {
        [SerializeField] private T _prefab;
        private readonly Queue<T> _poolList = new();
        public void PoolDestroy(T pool) {
            pool.gameObject.SetActive(false);
            pool.transform.SetParent(null);
            _poolList.Enqueue(pool);
        }
        public T PoolInstantiate() {
            if(_poolList.Count > 0) {
                var obj = _poolList.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }
            return Instantiate(_prefab);
        }
    }
}