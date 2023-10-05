using System;
using UnityEditor;

namespace MornDictionary
{
    [CustomEditor(typeof(MornDictionaryEnumBase<,>), true)]
    public sealed class MornDictionaryEnumEditor : MornDictionaryBaseEditor<int>
    {
        protected override int GetValue(SerializedProperty property)
        {
            return property.enumValueIndex;
        }

        protected override void AfterRenderDictionary()
        {
            var genericType = target.GetType().BaseType?.GetGenericArguments()[0];
            if (genericType == null)
            {
                return;
            }

            foreach (var enumValue in Enum.GetValues(genericType))
            {
                if (KeyNotFoundHashSet.Contains((int)enumValue))
                {
                    continue;
                }

                EditorGUILayout.HelpBox($"{enumValue} is not Registered", MessageType.Error);
            }
        }
    }
}