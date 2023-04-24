using UnityEngine;

namespace MornLib.Extensions
{
    public static class MornVector2Ex
    {
        public static float GetRandomValue(this Vector2 @base)
        {
            return Random.Range(@base.x, @base.y);
        }

        public static float Lerp(this Vector2 @base, float t)
        {
            return Mathf.Lerp(@base.x, @base.y, t);
        }
    }
}
