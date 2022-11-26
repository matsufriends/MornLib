using System;
using UnityEngine;

namespace MornLib.Cores
{
    [Serializable]
    public struct MornFloatRange
    {
        public float Start;
        public float End;

        public MornFloatRange(float start, float end)
        {
            Start = start;
            End = end;
        }

        public float Lerp(float rate)
        {
            return Mathf.Lerp(Start, End, Mathf.Clamp01(rate));
        }

        public float Clamp(float value)
        {
            return Mathf.Clamp(value, Start, End);
        }

        public float Random()
        {
            return UnityEngine.Random.Range(Start, End);
        }
    }
}