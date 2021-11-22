using System;
namespace MornLib.Cores {
    public static class MornMath {
        public static T MaxBy<T>(T a, T b, Func<T, int> func) {
            var valueA = func.Invoke(a);
            var valueB = func.Invoke(b);
            return valueA > valueB ? a : b;
        }
        public static T MinBy<T>(T a, T b, Func<T, int> func) {
            var valueA = func.Invoke(a);
            var valueB = func.Invoke(b);
            return valueA < valueB ? a : b;
        }
    }
}