using UnityEngine;

namespace MornAttribute
{
    public sealed class HelpBoxAttribute : PropertyAttribute
    {
        public readonly float Height;
        public readonly HelpBoxType HelpBoxType;
        public readonly string LabelName;

        public HelpBoxAttribute(string labelName, float height = 30, HelpBoxType helpBoxType = HelpBoxType.Info)
        {
            LabelName = labelName;
            Height = height;
            HelpBoxType = helpBoxType;
        }
    }
}