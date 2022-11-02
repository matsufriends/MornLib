using System.Collections.Generic;
using MornLib.Pool;
namespace MornLib.Cores {
    public class MornHashSet<T> : HashSet<T>,IPoolItem {
        void IPoolItem.Clear() => Clear();
    }
}