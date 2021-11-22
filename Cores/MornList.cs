using System.Collections.Generic;
namespace MornLib.Cores {
    public class MornList<T> : List<T>,IPool {
        void IPool.Clear() {
            Clear();
        }
    }
}