using UnityEngine;

namespace MornUI
{
    internal static class MornUIUtil
    {
        internal static MornUIDirType ToDir(this Vector2 value)
        {
            if (value.sqrMagnitude < 0.5f) return MornUIDirType.None;

            if (Mathf.Abs(value.x) <= Mathf.Abs(value.y)) return 0 < value.y ? MornUIDirType.Up : MornUIDirType.Down;

            return 0 < value.x ? MornUIDirType.Right : MornUIDirType.Left;
        }
    }
}