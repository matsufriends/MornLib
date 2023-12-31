using System;
using UnityEditor;
using UnityEngine;

namespace MornEditor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    internal sealed class HelpBoxDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var atr = (HelpBoxAttribute)attribute;
            var infoBoxRect = new Rect(position.x, position.y, position.width, atr.Height);
            var messageType = atr.HelpBoxType switch
            {
                HelpBoxType.None    => MessageType.None,
                HelpBoxType.Info    =>  MessageType.Info,
                HelpBoxType.Warning =>  MessageType.Warning,
                HelpBoxType.Error   =>  MessageType.Error,
                _ => throw new ArgumentOutOfRangeException(),
            };
            EditorGUI.HelpBox(infoBoxRect, atr.LabelName, messageType);
            var propertyRect = new Rect(position.x, position.y + atr.Height, position.width, position.height);
            EditorGUI.PropertyField(propertyRect, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var atr = (HelpBoxAttribute)attribute;
            return base.GetPropertyHeight(property, label) + atr.Height;
        }
    }
}