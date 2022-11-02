using System;
using System.Collections.Generic;
namespace MornLib.Pool {
    public sealed class MornObjectPool<T> {
        private readonly Func<T> _generator;
        private readonly Action<T> _onRent;
        private readonly Action<T> _onReturn;
        private readonly Stack<T> _poolStack = new();
        public MornObjectPool(Func<T> generator,Action<T> onRent,Action<T> onReturn,int startCount) {
            if(startCount < 0) throw new ArgumentOutOfRangeException();
            _generator = generator;
            _onRent    = onRent;
            _onReturn  = onReturn;
            for(var i = 0;i < startCount;i++) {
                var generated = generator();
                onReturn(generated);
                _poolStack.Push(generated);
            }
        }
        public T Rent() {
            if(_poolStack.TryPop(out var result) == false) result = _generator();
            _onRent(result);
            return result;
        }
        public void Return(T pushObject) {
            _onReturn(pushObject);
            _poolStack.Push(pushObject);
        }
    }
}