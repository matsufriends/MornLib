using UnityEngine;
namespace MornLib.Extensions {
    public static class TransformEx {
        public static void DestroyChildren(this Transform transform) {
            var totalCount = transform.childCount;
            for(var i = totalCount - 1;i >= 0;i--) Object.DestroyImmediate(transform.GetChild(i).gameObject);
        }
        public static Vector3 GetConvertedDifUsingLocalAxis(this Transform transform,Vector3 dif) {
            return transform.right * dif.x + transform.up * dif.y + transform.forward * dif.z;
        }
    }
}