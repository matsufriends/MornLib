using UnityEngine;

namespace MornAttribute
{
    public sealed class MinMaxSliderAttribute : PropertyAttribute
    {
        public readonly float Max;
        public readonly float Min;

        public MinMaxSliderAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}