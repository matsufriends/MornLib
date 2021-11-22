using System.Collections.Generic;
namespace MornLib.Cores {
    public class MornHashSet<T> : HashSet<T>,IPool {
        void IPool.Clear() {
            Clear();
        }
    }
}