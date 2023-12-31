using MornAttribute;
using UnityEditor;

namespace MornEditor
{
    [CustomPropertyDrawer(typeof(DisableIfAttribute))]
    internal sealed class DisableIfDrawer : EnableOrDisableDrawerBase
    {
        protected override bool CorrectValue => false;
    }
}
