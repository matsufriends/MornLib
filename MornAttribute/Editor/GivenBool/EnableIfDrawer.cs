using UnityEditor;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    internal sealed class EnableIfDrawer : EnableOrDisableDrawerBase
    {
        protected override bool CorrectValue => true;
    }
}