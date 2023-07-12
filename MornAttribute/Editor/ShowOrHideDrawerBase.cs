using UnityEditor;
using UnityEngine;

namespace MornAttribute
{
    public abstract class ShowOrHideDrawerBase : GivenBoolNameDrawerBase
    {
        protected override void OnCorrectValue(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        protected override void OnIncorrectValue(Rect position, SerializedProperty property, GUIContent label)
        {
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (TryGetBool(property, out var boolValue) && boolValue != CorrectValue)
            {
                return -2;
            }

            return base.GetPropertyHeight(property, label);
        }
    }
}
