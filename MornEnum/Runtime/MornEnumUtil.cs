using System;
using System.Collections.Generic;

namespace MornEnum.Runtime
{
    /// <summary>enumに関するUtility</summary>
    /// <typeparam name="T">enum</typeparam>
    public static class MornEnumUtil<T> where T : Enum
    {
        /// <summary>enumの全要素リスト</summary>
        private static List<T> s_enumList;

        /// <summary>enumをstring化した結果を保存するDictionary</summary>
        private static readonly Dictionary<T, string> s_toStringDictionary = new();

        /// <summary>enumの全項目数</summary>
        public static int Count => Values.Count;

        /// <summary>全項目を含むIReadOnlyListを返す</summary>
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

        /// <summary>ToStringの結果をキャッシュして返す</summary>
        /// <param name="value">enum</param>
        /// <returns>文字列</returns>
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
