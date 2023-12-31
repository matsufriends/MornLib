using UnityEditor;
using UnityEngine;

namespace MornEditor
{
    internal abstract class ShowOrHideDrawerBase : GivenBoolDrawerBase
    {
        protected override void OnCorrect(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        protected override void OnIncorrect(Rect position, SerializedProperty property, GUIContent label)
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