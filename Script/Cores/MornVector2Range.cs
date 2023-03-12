using System;
using UnityEngine;

namespace MornLib.Cores
{
    [Serializable]
    public struct MornVector2Range
    {
        public Vector2 Start;
        public Vector2 End;

        public MornVector2Range(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public Vector2 Lerp(float rate)
        {
            return Vector2.Lerp(Start, End, Mathf.Clamp01(rate));
        }
    }
}
