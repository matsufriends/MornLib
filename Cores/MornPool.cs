using System;
using System.Collections.Generic;
namespace MornLib.Cores {
    public static class MornPool<T> where T : IPool,new() {
        private static readonly Queue<T>   s_poolList = new Queue<T>();
        private static readonly HashSet<T> s_hashSet  = new HashSet<T>();
        private static          int        s_conflictCount;
        private const           int        _maxConflict = 1000;
        public static T Rent() {
            var result = s_poolList.Count > 0 ? s_poolList.Dequeue() : new T();
            if(s_hashSet.Add(result) == false) {
                s_conflictCount++;
                if(s_conflictCount > _maxConflict) throw new Exception("衝突しすぎ;;");
                Rent();
            }
            result.Clear();
            return result;
        }
        public static void Return(T item) {
            if(s_hashSet.Remove(item) == false) throw new Exception("Pool管理外のInstanceが渡された");
            s_poolList.Enqueue(item);
        }
    }
}