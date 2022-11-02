using MornLib.Singletons;
using UnityEngine;
namespace MornLib.Pool {
    public abstract class MornBaseObjectPoolMono<T> : SingletonMono<MornBaseObjectPoolMono<T>> where T : MonoBehaviour {
        [SerializeField] private T _cellPrefab;
        private MornObjectPool<T> _mornObjectPool;
        protected override void MyAwake() => _mornObjectPool = new MornObjectPool<T>(
                                                 () => Instantiate(_cellPrefab,transform),x => {
                                                     x.transform.SetParent(transform);
                                                     x.gameObject.SetActive(true);
                                                 },x => {
                                                     x.gameObject.SetActive(false);
                                                     x.transform.SetParent(transform);
                                                 },50
                                             );
        public T Rent() => _mornObjectPool.Rent();
        public void Return(T poolObject) => _mornObjectPool.Return(poolObject);
    }
}