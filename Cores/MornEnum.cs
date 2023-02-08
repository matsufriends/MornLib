using System;
using System.Collections.Generic;

namespace MornLib.Cores
{
    public static class MornEnum<T> where T : Enum
    {
        private static readonly Dictionary<T, string> _toStringDictionary = new();
        private static List<T> s_enumList;
        public static int Count => Values.Count;

        public static IReadOnlyList<T> Values
        {
            get
            {
                if (s_enumList == null)
                {
                    s_enumList = new List<T>();
                    foreach (var value in Enum.GetValues(typeof(T)))
                    {
                        s_enumList.Add((T)value);
                    }
                }

                return s_enumList;
            }
        }

        public static string CachedToString(T value)
        {
            if (_toStringDictionary.TryGetValue(value, out var st))
            {
                return st;
            }

            _toStringDictionary.Add(value, value.ToString());
            return _toStringDictionary[value];
        }
    }
}
