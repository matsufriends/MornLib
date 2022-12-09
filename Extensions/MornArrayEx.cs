using System;
using Random = UnityEngine.Random;

namespace MornLib.Extensions
{
    public static class MornArrayEx
    {
        public static bool Contains<T>(this T[] array, T value)
        {
            return Array.IndexOf(array, value) >= 0;
        }

        public static T RandomValue<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }
    }
}
