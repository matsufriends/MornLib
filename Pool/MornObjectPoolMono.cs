using MornLib.Singletons;
using UnityEngine;
namespace MornLib.Pool {
    public class MornObjectPoolMono<T> : SingletonMono<MornObjectPoolMono<T>> where T : MonoBehaviour {
        [SerializeField] private T _cellPrefab;
        private MornObjectPool<T> _mornObjectPool;
        protected override void MyAwake() => _mornObjectPool = new MornObjectPool<T>(OnGenerate,OnRent,OnReturn,StartCount);
        protected virtual int StartCount => 10;
        protected virtual void OnReturn(T x) {
            x.gameObject.SetActive(false);
            x.transform.SetParent(transform);
        }
        protected virtual void OnRent(T x) {
            x.transform.SetParent(transform);
            x.gameObject.SetActive(true);
        }
        protected virtual T OnGenerate() => Instantiate(_cellPrefab,transform);
        public T Rent() => _mornObjectPool.Rent();
        public void Return(T poolObject) => _mornObjectPool.Return(poolObject);
    }
}