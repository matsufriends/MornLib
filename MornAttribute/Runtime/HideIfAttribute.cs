using UnityEngine;

namespace MornAttribute
{
    public sealed class HideIfAttribute : PropertyAttribute
    {
        public readonly string PropertyName;

        public HideIfAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
