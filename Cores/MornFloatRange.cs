using UnityEngine;
namespace MornLib.Cores {
    [System.Serializable]
    public struct MornFloatRange {
        public float Start;
        public float End;
        public MornFloatRange(float start,float end) {
            Start = start;
            End = end;
        }
        public float Lerp(float rate) {
            return Mathf.Lerp(Start,End,Mathf.Clamp01(rate));
        }
    }
}