using UnityEditor;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    internal sealed class HideIfDrawer : ShowOrHideDrawerBase
    {
        protected override bool CorrectValue => false;
    }
}