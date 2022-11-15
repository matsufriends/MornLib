using UnityEngine;
namespace MornLib.Extensions {
    public static class FloatEx {
        public static bool AsProbability(this float value) => Random.Range(0,1f) <= value;
    }
}
