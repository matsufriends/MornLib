using UnityEngine;
namespace MornLib.Mono {
    public interface IPool<T> where T : MonoBehaviour {
        void       PoolDestroy(T pool);
        T PoolInstantiate();
    }
}