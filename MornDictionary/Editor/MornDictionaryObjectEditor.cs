using UnityEditor;
using UnityEngine;

namespace MornDictionary
{
    [CustomEditor(typeof(MornDictionaryObjectBase<,>), true)]
    public sealed class MornDictionaryObjectEditor : MornDictionaryBaseEditor<Object>
    {
        protected override Object GetValue(SerializedProperty property)
        {
            return property.objectReferenceValue;
        }

        protected override void AfterRenderDictionary()
        {
        }
    }
}
