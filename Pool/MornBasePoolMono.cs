using System;
using System.Collections.Generic;
namespace MornLib.Pool {
    public sealed class MornObjectPool<T> {
        private readonly Stack<T> _poolStack = new();
        private readonly Func<T> _generator;
        private readonly Action<T> _onPop;
        private readonly Action<T> _onPush;
        public MornObjectPool(Func<T> generator,Action<T> onPop,Action<T> onPush,uint startCount) {
            _generator = generator;
            _onPop      = onPop;
            _onPush     = onPush;
            for(var i = 0;i < startCount;i++) {
                var generated = generator();
                onPush(generated);
                _poolStack.Push( generated);
            }
        }
        public T Pop() {
            if(_poolStack.TryPop(out var result) == false) result =_generator();
            _onPop(result);
            return result;
        }
        public void Push(T pushObject) {
            _onPush(pushObject);
            _poolStack.Push(pushObject);
        }
    }
}