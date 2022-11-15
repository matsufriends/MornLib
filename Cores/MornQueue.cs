using System.Collections.Generic;
using MornLib.Pool;
namespace MornLib.Cores {
    public class MornQueue<T> : Queue<T>,IPoolItem {
        void IPoolItem.Clear() => Clear();
    }
}
