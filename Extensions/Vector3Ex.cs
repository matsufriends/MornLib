using UnityEngine;
namespace MornLib.Extensions {
    public static class Vector3Ex {
        public static Vector2 XZ(this Vector3 vector3) {
            return new Vector2(vector3.x, vector3.z);
        }
    }
}