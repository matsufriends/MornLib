using UnityEditor;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    internal sealed class ShowIfDrawer : ShowOrHideDrawerBase
    {
        protected override bool CorrectValue => true;
    }
}