using System;
using UnityEngine;
namespace MornLib.Cores {
    public static class MornMath {
        public static T MaxBy<T>(T a,T b,Func<T,int> func) {
            var valueA = func.Invoke(a);
            var valueB = func.Invoke(b);
            return valueA > valueB ? a : b;
        }
        public static T MinBy<T>(T a,T b,Func<T,int> func) {
            var valueA = func.Invoke(a);
            var valueB = func.Invoke(b);
            return valueA < valueB ? a : b;
        }
        public static float LerpRadian(float a,float b,float t) {
            return Mathf.LerpAngle(a * Mathf.Rad2Deg,b * Mathf.Rad2Deg,t) * Mathf.Deg2Rad;
        }
        public static bool IsNearZero(float a) {
            return Mathf.Abs(a) <= 0.0001f;
        }
    }
}