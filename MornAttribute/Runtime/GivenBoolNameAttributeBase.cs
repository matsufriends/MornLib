using UnityEngine;

namespace MornAttribute
{
    public abstract class GivenBoolNameAttributeBase : PropertyAttribute
    {
        public readonly string PropertyName;

        public GivenBoolNameAttributeBase(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
