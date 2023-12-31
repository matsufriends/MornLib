using System.Reflection;
using MornAttribute;
using UnityEditor;
using UnityEngine;

namespace MornEditor
{
    internal abstract class GivenBoolDrawerBase : PropertyDrawer
    {
        protected abstract bool CorrectValue { get; }
        protected abstract void OnCorrect(Rect position, SerializedProperty property, GUIContent label);
        protected abstract void OnIncorrect(Rect position, SerializedProperty property, GUIContent label);

        protected bool TryGetBool(SerializedProperty property, out bool value)
        {
            var showIf = (GivenBoolNameAttributeBase)attribute;
            var targetObject = property.serializedObject.targetObject;
            var propertyInfo = targetObject.GetType().GetProperty(showIf.PropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyInfo != null && propertyInfo.GetValue(targetObject) is bool boolValue)
            {
                value = boolValue;
                return true;
            }

            value = false;
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (TryGetBool(property, out var boolValue))
            {
                if (boolValue == CorrectValue)
                {
                    OnCorrect(position, property, label);
                }
                else
                {
                    OnIncorrect(position, property, label);
                }
            }
            else
            {
                EditorGUI.HelpBox(position, $"Property not found: {((GivenBoolNameAttributeBase)attribute).PropertyName}", MessageType.Error);
            }
        }
    }
}
