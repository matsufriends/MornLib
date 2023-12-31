using MornAttribute;
using UnityEditor;

namespace MornEditor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    internal sealed class ShowIfDrawer : ShowOrHideDrawerBase
    {
        protected override bool CorrectValue => true;
    }
}
