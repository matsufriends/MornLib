namespace MornAttribute
{
    public sealed class ShowIfAttribute : GivenBoolNameAttributeBase
    {
        public ShowIfAttribute(string propertyName) : base(propertyName)
        {
        }
    }
}