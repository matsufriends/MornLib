using UnityEditor;
using UnityEngine;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public sealed class LabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var labelAttribute = (LabelAttribute)attribute;
            EditorGUI.PropertyField(position, property, new GUIContent(labelAttribute.LabelName));
        }
    }
}
