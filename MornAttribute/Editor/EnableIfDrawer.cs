using UnityEditor;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    public sealed class EnableIfDrawer : EnableOrDisableDrawerBase
    {
        protected override bool CorrectValue => true;
    }
}
