using UnityEditor;
using UnityEngine;

namespace MornAttribute
{
    public abstract class EnableOrDisableDrawerBase : GivenBoolNameDrawerBase
    {
        protected override void OnCorrectValue(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        protected override void OnIncorrectValue(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
