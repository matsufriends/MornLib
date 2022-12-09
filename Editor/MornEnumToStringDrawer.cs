using System;
using MornLib.Attribute;
using UnityEditor;
using UnityEngine;

namespace MornLib.Editor
{
    [CustomPropertyDrawer(typeof(MornEnumToStringAttribute))]
    public class MornEnumToStringDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var enumType = ((MornEnumToStringAttribute)attribute).EnumType;
                Enum cur;
                if (Enum.IsDefined(enumType, property.stringValue))
                {
                    cur = (Enum)Enum.Parse(enumType, property.stringValue);
                }
                else
                {
                    cur = (Enum)Activator.CreateInstance(enumType);
                }

                var value = EditorGUI.EnumPopup(position, label, cur);
                property.stringValue = value.ToString();
            }
        }
    }
}
