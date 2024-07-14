using MornLib.Cores;
using MornLib.Pools;
using UnityEngine;

namespace MornLib.Extensions
{
    public static class MornTransformEx
    {
        public static void DestroyChildren(this Transform transform)
        {
            var totalCount = transform.childCount;
            for (var i = totalCount - 1; i >= 0; i--) Object.DestroyImmediate(transform.GetChild(i).gameObject);
        }

        public static string GetPath(this Transform transform)
        {
            var list = MornSharedObjectPool<MornList<string>>.Rent();
            var builder = MornSharedObjectPool<MornStringBuilder>.Rent();
            builder.Init('/');
            var parent = transform;
            while (parent != null)
            {
                list.Add(parent.name);
                parent = parent.parent;
            }

            for (var i = list.Count - 1; i >= 0; i--) builder.Append(list[i]);

            var message = builder.Get();
            MornSharedObjectPool<MornList<string>>.Return(list);
            MornSharedObjectPool<MornStringBuilder>.Return(builder);
            return message;
        }

        public static Vector3 GetConvertedDifUsingLocalAxis(this Transform transform, Vector3 dif)
        {
            return transform.right * dif.x + transform.up * dif.y + transform.forward * dif.z;
        }

        public static void PositionLerp(this Transform transform, Vector3 aim, float k)
        {
            transform.position = Vector3.Lerp(transform.position, aim, Mathf.Clamp01(k));
        }

        public static void LocalPositionLerp(this Transform transform, Vector3 aim, float k)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aim, Mathf.Clamp01(k));
        }

        public static void RotationLerp(this Transform transform, Quaternion aim, float k)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, aim, Mathf.Clamp01(k));
        }

        public static void LocalRotationLerp(this Transform transform, Quaternion aim, float k)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, aim, Mathf.Clamp01(k));
        }

        public static void EulerAnglesLerp(this Transform transform, Vector3 aim, float k)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, aim, Mathf.Clamp01(k));
        }

        public static void LocalEulerAnglesLerp(this Transform transform, Vector3 aim, float k)
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, aim, Mathf.Clamp01(k));
        }
    }
}