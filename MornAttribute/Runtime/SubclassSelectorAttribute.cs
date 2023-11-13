using UnityEngine;

namespace MornAttribute
{
    public sealed class SubclassSelectorAttribute : PropertyAttribute
    {
        private readonly bool _includeMono;

        public SubclassSelectorAttribute(bool includeMono = false)
        {
            _includeMono = includeMono;
        }

        public bool IsIncludeMono()
        {
            return _includeMono;
        }
    }
}