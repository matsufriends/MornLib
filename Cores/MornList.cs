using System.Collections.Generic;
using MornLib.Pool;
namespace MornLib.Cores {
    public class MornList<T> : List<T>,IPoolItem {
        void IPoolItem.Clear() => Clear();
    }
}