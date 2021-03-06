using UnityEngine;
namespace MornLib.Extensions {
    public static class Vector2IntEx {
        public static Vector3 XZ(this Vector2Int @base) {
            return new Vector3(@base.x, 0, @base.y);
        }
        public static int Min(this Vector2Int @base) {
            return @base.x < @base.y ? @base.x : @base.y;
        }
        public static int Max(this Vector2Int @base) {
            return @base.x < @base.y ? @base.y : @base.x;
        }
    }
}