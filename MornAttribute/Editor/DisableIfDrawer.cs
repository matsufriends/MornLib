using UnityEditor;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(DisableIfAttribute))]
    public sealed class DisableIfDrawer : EnableOrDisableDrawerBase
    {
        protected override bool CorrectValue => false;
    }
}
