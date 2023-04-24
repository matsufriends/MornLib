using System.Linq;
using MornLib.Extensions;

namespace MornLib.Cores
{
    public static class MornRandom
    {
        public static T Random<T>(params T[] args)
        {
            return args.RandomValue();
        }

        public static T Random<T>(params (T, float)[] args)
        {
            var random = UnityEngine.Random.value * args.Sum(pair => pair.Item2);
            foreach (var pair in args)
            {
                if (pair.Item2 < random)
                {
                    return pair.Item1;
                }
            }

            return args[^1].Item1;
        }
    }
}
