using System.Collections.Generic;
namespace MornLib.Cores {
    public class MornQueue<T> : Queue<T>,IPool {
        void IPool.Clear() {
            Clear();
        }
    }
}