using UnityEngine;
namespace MornLib.Mono {
    public interface IPool<T> where T : MonoBehaviour {
        public void       PoolDestroy(T pool);
        public T PoolInstantiate();
    }
}