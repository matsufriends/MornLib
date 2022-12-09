using UnityEngine;

namespace MornLib.Extensions
{
    public static class MornVector3Ex
    {
        public static Vector2 XZ(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }

        public static Vector3 Random(float value)
        {
            var x = UnityEngine.Random.Range(-value, value);
            var y = UnityEngine.Random.Range(-value, value);
            var z = UnityEngine.Random.Range(-value, value);
            return new Vector3(x, y, z);
        }
    }
}
