using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MornEditor
{
    [CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
    internal class SubclassSelectorDrawer : PropertyDrawer
    {
        private bool _initialized;
        private Type[] _inheritedTypes;
        private string[] _typePopupNameArray;
        private string[] _typeFullNameArray;
        private int _currentTypeIndex;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference)
            {
                return;
            }

            if (!_initialized)
            {
                Initialize(property);
                _initialized = true;
            }

            GetCurrentTypeIndex(property.managedReferenceFullTypename);
            var selectedTypeIndex = EditorGUI.Popup(GetPopupPosition(position), _currentTypeIndex, _typePopupNameArray);
            UpdatePropertyToSelectedTypeIndex(property, selectedTypeIndex);
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }

        private void Initialize(SerializedProperty property)
        {
            var utility = (SubclassSelectorAttribute)attribute;
            GetAllInheritedTypes(GetFieldType(property), utility.IsIncludeMono());
            GetInheritedTypeNameArrays();
        }

        private void GetCurrentTypeIndex(string typeFullName)
        {
            _currentTypeIndex = Array.IndexOf(_typeFullNameArray, typeFullName);
        }

        private void GetAllInheritedTypes(Type baseType, bool includeMono)
        {
            var monoType = typeof(MonoBehaviour);
            _inheritedTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => baseType.IsAssignableFrom(p) && p.IsClass && (!monoType.IsAssignableFrom(p) || includeMono))
                    .Prepend(null)
                    .ToArray();
        }

        private void GetInheritedTypeNameArrays()
        {
            _typePopupNameArray = _inheritedTypes.Select(type =>
                    {
                        var attribute = type?.GetCustomAttribute<SubclassNameAttribute>();
                        return type == null ? "<null>" :
                                attribute == null ? type.Name : attribute.Name;
                    })
                    .ToArray();
            _typeFullNameArray = _inheritedTypes.Select(type => type == null ? "" : $"{type.Assembly.ToString().Split(',')[0]} {type.FullName}")
                    .ToArray();
        }

        private static Type GetFieldType(SerializedProperty property)
        {
            var fieldTypename = property.managedReferenceFieldTypename.Split(' ');
            var assembly = Assembly.Load(fieldTypename[0]);
            return assembly.GetType(fieldTypename[1]);
        }

        private void UpdatePropertyToSelectedTypeIndex(SerializedProperty property, int selectedTypeIndex)
        {
            if (_currentTypeIndex == selectedTypeIndex)
            {
                return;
            }

            _currentTypeIndex = selectedTypeIndex;
            var selectedType = _inheritedTypes[selectedTypeIndex];
            property.managedReferenceValue = selectedType == null ? null : Activator.CreateInstance(selectedType);
        }

        private static Rect GetPopupPosition(Rect currentPosition)
        {
            var popupPosition = new Rect(currentPosition);
            popupPosition.width -= EditorGUIUtility.labelWidth;
            popupPosition.x += EditorGUIUtility.labelWidth;
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            return popupPosition;
        }
    }
}