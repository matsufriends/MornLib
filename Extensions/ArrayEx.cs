using System;
using Random = UnityEngine.Random;
namespace MornLib.Extensions {
    public static class ArrayEx {
        public static bool Contains<T>(this T[] array,T value) where T : IEquatable<T> {
            for(var i = 0;i < array.Length;i++) {
                if(array[i].Equals(value)) return true;
            }
            return false;
        }

        public static bool EnumContains<T>(this T[] array,T value) where T : Enum {
            for(var i = 0;i < array.Length;i++) {
                if(array[i].Equals(value)) return true;
            }
            return false;
        }

        public static T RandomValue<T>(this T[] array) {
            return array[Random.Range(0,array.Length)];
        }
    }
}
