using UnityEngine;

namespace MornEditor
{
    public sealed class HelpBoxAttribute : PropertyAttribute
    {
        public readonly string LabelName;
        public readonly float Height;
        public readonly HelpBoxType HelpBoxType;

        public HelpBoxAttribute(string labelName, float height = 30, HelpBoxType helpBoxType = HelpBoxType.Info)
        {
            LabelName = labelName;
            Height = height;
            HelpBoxType = helpBoxType;
        }
    }
}