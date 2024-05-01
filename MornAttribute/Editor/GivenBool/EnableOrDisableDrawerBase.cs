using UnityEditor;
using UnityEngine;

namespace MornAttribute
{
    internal abstract class EnableOrDisableDrawerBase : GivenBoolDrawerBase
    {
        protected override void OnCorrect(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        protected override void OnIncorrect(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}