using UnityEngine;

namespace MornEditor
{
    public sealed class LabelAttribute : PropertyAttribute
    {
        public readonly string LabelName;

        public LabelAttribute(string labelName)
        {
            LabelName = labelName;
        }
    }
}
