using UnityEngine;

namespace MornLib.Pool
{
    public abstract class MornBaseObjectPoolMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T _prefab;
        private MornObjectPool<T> _mornObjectPool;

        private void Awake()
        {
            _mornObjectPool = new MornObjectPool<T>(OnGenerate, OnRent, OnReturn, StartCount);
        }

        protected virtual int StartCount => 10;

        protected virtual void OnReturn(T x)
        {
            x.gameObject.SetActive(false);
            x.transform.SetParent(transform);
        }

        protected virtual void OnRent(T x)
        {
            x.transform.SetParent(transform);
            x.gameObject.SetActive(true);
        }

        protected virtual T OnGenerate()
        {
            return Instantiate(_prefab, transform);
        }

        public T Rent()
        {
            return _mornObjectPool.Rent();
        }

        public void Return(T poolObject)
        {
            _mornObjectPool.Return(poolObject);
        }
    }
}
