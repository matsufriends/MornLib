using MornLib.Singletons;
using UnityEngine;
namespace MornLib.Pool {
    public class MornObjectPoolMono<T> : SingletonMono<MornObjectPoolMono<T>> where T : MonoBehaviour {
        [SerializeField] private T _cellPrefab;
        private MornObjectPool<T> _mornObjectPool;
        protected override void MyAwake() {
            _mornObjectPool = new MornObjectPool<T>(
                () => Instantiate(_cellPrefab,transform),x => {
                    x.transform.SetParent(transform);
                    x.gameObject.SetActive(true);
                },x => {
                    x.gameObject.SetActive(false);
                    x.transform.SetParent(transform);
                },50
            );
        }
        public T Pop() {
            return _mornObjectPool.Pop();
        }
        public void Push(T poolObject) {
            _mornObjectPool.Push(poolObject);
        }
    }
}