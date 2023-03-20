using System;
using UnityEditor;
using UnityEngine;

namespace MornEnum
{
    [CustomPropertyDrawer(typeof(MornEnumToStringAttribute))]
    internal sealed class MornEnumToStringDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enumType = ((MornEnumToStringAttribute)attribute).EnumType;
            if (enumType == null || enumType.IsEnum == false)
            {
                EditorGUI.HelpBox(position, $"Attribute argument must be enum: {enumType}", MessageType.Error);
                return;
            }

            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.HelpBox(position, $"Property type must be string: {property.propertyType}", MessageType.Error);
                return;
            }

            Enum selected;
            if (Enum.IsDefined(enumType, property.stringValue))
            {
                selected = (Enum)Enum.Parse(enumType, property.stringValue);
            }
            else
            {
                selected = (Enum)Activator.CreateInstance(enumType);
            }

            var value = EditorGUI.EnumPopup(position, label, selected);
            property.stringValue = value.ToString();
        }
    }
}
