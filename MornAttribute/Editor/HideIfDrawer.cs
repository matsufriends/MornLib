using UnityEditor;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public sealed class HideIfDrawer : ShowOrHideDrawerBase
    {
        protected override bool CorrectValue => false;
    }
}
