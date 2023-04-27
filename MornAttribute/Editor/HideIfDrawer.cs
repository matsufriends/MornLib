using UnityEditor;
using UnityEngine;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public sealed class HideIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var hideIf = (HideIfAttribute)attribute;
            var conditionProperty = property.serializedObject.FindProperty(hideIf.PropertyName);
            if (conditionProperty == null)
            {
                EditorGUI.HelpBox(position, $"Property not found: {hideIf.PropertyName}", MessageType.Error);
            }
            else if (conditionProperty.propertyType != SerializedPropertyType.Boolean)
            {
                EditorGUI.HelpBox(position, $"Property type must be bool: {conditionProperty.propertyType}", MessageType.Error);
            }
            else
            {
                var conditionValue = conditionProperty.boolValue;
                if (!conditionValue)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hideIf = (HideIfAttribute)attribute;
            var conditionProperty = property.serializedObject.FindProperty(hideIf.PropertyName);
            if (conditionProperty == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            if (conditionProperty.propertyType != SerializedPropertyType.Boolean)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            var conditionValue = conditionProperty.boolValue;
            if (!conditionValue)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }

            return 0;
        }
    }
}
