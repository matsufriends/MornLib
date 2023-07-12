using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MornAttribute
{
    public abstract class GivenBoolNameDrawerBase : PropertyDrawer
    {
        protected abstract bool CorrectValue { get; }
        protected abstract void OnCorrectValue(Rect position, SerializedProperty property, GUIContent label);
        protected abstract void OnIncorrectValue(Rect position, SerializedProperty property, GUIContent label);

        protected bool TryGetBool(SerializedProperty property, out bool value)
        {
            var showIf = (GivenBoolNameAttributeBase)attribute;
            var targetObject = property.serializedObject.targetObject;
            var propertyInfo = targetObject.GetType().GetProperty(showIf.PropertyName, BindingFlags.Instance | BindingFlags.NonPublic);
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
                    OnCorrectValue(position, property, label);
                }
                else
                {
                    OnIncorrectValue(position, property, label);
                }
            }
            else
            {
                EditorGUI.HelpBox(position, $"Property not found: {((GivenBoolNameAttributeBase)attribute).PropertyName}", MessageType.Error);
            }
        }
    }
}
