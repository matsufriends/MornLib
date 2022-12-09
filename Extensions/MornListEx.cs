using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace MornLib.Extensions
{
    public static class MornListEx
    {
        public static T RandomValue<T>(this IReadOnlyList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static int MatchCount<T>(this IReadOnlyList<T> list, T correct)
        {
            var count = 0;
            var totalCount = list.Count;
            for (var i = totalCount - 1; i >= 0; i--)
            {
                if (EqualityComparer<T>.Default.Equals(list[i], correct))
                {
                    count++;
                }
            }

            return count;
        }

        public static int EnumMatchCount<T>(this IReadOnlyList<T> list, T correct) where T : Enum
        {
            var count = 0;
            var totalCount = list.Count;
            for (var i = totalCount - 1; i >= 0; i--)
            {
                if (EqualityComparer<T>.Default.Equals(list[i], correct))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
