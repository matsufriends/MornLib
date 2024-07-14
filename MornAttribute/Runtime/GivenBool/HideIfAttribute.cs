namespace MornAttribute
{
    public sealed class HideIfAttribute : GivenBoolNameAttributeBase
    {
        public HideIfAttribute(string propertyName) : base(propertyName)
        {
        }
    }
}