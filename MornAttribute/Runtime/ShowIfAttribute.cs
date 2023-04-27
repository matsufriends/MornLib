using UnityEngine;

namespace MornAttribute
{
    public sealed class ShowIfAttribute : PropertyAttribute
    {
        public readonly string PropertyName;

        public ShowIfAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
