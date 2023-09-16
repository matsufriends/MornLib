using System;
using System.Collections.Generic;

namespace MornEnum
{
    public static class MornEnumUtil<T> where T : Enum
    {
        private static List<T> s_enumList;
        private static readonly Dictionary<T, string> s_toStringDictionary = new();
        public static int Count => Values.Count;
        public static IReadOnlyList<T> Values
        {
            get
            {
                if (s_enumList != null)
                {
                    return s_enumList;
                }

                s_enumList = new List<T>();
                foreach (var value in Enum.GetValues(typeof(T)))
                {
                    s_enumList.Add((T)value);
                }

                return s_enumList;
            }
        }

        public static string CachedToString(T value)
        {
            if (s_toStringDictionary.TryGetValue(value, out var st))
            {
                return st;
            }

            s_toStringDictionary.Add(value, value.ToString());
            return s_toStringDictionary[value];
        }
    }
}