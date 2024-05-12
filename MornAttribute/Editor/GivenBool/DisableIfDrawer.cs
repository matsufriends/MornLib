using UnityEditor;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(DisableIfAttribute))]
    internal sealed class DisableIfDrawer : EnableOrDisableDrawerBase
    {
        protected override bool CorrectValue => false;
    }
}