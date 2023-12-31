using MornAttribute;
using UnityEditor;

namespace MornEditor
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    internal sealed class HideIfDrawer : ShowOrHideDrawerBase
    {
        protected override bool CorrectValue => false;
    }
}
