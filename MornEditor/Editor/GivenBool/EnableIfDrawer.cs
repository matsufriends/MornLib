using MornAttribute;
using UnityEditor;

namespace MornEditor
{
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    internal sealed class EnableIfDrawer : EnableOrDisableDrawerBase
    {
        protected override bool CorrectValue => true;
    }
}
