using System;
using System.Collections.Generic;
namespace MornLib.Pool {
    public sealed class MornObjectPool<T> {
<<<<<<< Updated upstream:Pool/MornBasePoolMono.cs
        private readonly Stack<T> _poolStack = new();
        private readonly Func<T> _generate;
        private readonly Action<T> _onPop;
        private readonly Action<T> _onPush;
        public MornObjectPool(Func<T> generate,Action<T> onPop,Action<T> onPush,uint startCount) {
            _generate = generate;
            _onPop      = onPop;
            _onPush     = onPush;
            for(var i = 0;i < startCount;i++) {
                var generated = generate();
                onPush(generated);
                _poolStack.Push( generated);
            }
        }
        public T Pop() {
            if(_poolStack.TryPop(out var result) == false) result =_generate();
            _onPop(result);
=======
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
>>>>>>> Stashed changes:Pool/MornObjectPool.cs
            return result;
        }
        public void Return(T pushObject) {
            _onReturn(pushObject);
            _poolStack.Push(pushObject);
        }
    }
}