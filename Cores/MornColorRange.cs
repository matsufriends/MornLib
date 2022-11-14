using UnityEngine;
namespace MornLib.Cores {
    [System.Serializable]
    public struct MornColorRange {
        public Color Start;
        public Color End;
        public MornColorRange(Color start,Color end) {
            Start = start;
            End   = end;
        }
        public Color Lerp(float rate) => Color.Lerp(Start,End,Mathf.Clamp01(rate));
    }
}