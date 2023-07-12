using UnityEditor;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public sealed class ShowIfDrawer : ShowOrHideDrawerBase
    {
        protected override bool CorrectValue => true;
    }
}
