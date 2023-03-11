using System;
using System.Collections.Generic;

namespace MornEnum.Runtime
{
    public static class MornEnumUtil<T> where T : Enum
    {
        private static readonly Dictionary<T, string> s_ToStringDictionary = new();
        private static List<T> s_enumList;
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
            if (s_ToStringDictionary.TryGetValue(value, out var st))
            {
                return st;
            }

            s_ToStringDictionary.Add(value, value.ToString());
            return s_ToStringDictionary[value];
        }
    }
}
