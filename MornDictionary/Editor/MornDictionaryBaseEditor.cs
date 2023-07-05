using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MornDictionary
{
    [CustomEditor(typeof(MornDictionaryBase<,>), true)]
    public sealed class MornDictionaryBaseEditor : Editor
    {
        private SerializedProperty _script;
        private SerializedProperty _keyList;
        private SerializedProperty _valueList;
        private readonly HashSet<int> _keyDuplicateHashSet = new();
        private readonly HashSet<int> _keyNotFoundHashSet = new();
        private const int ButtonSize = 20;

        private void OnEnable()
        {
            _script = serializedObject.FindProperty("m_Script");
            _keyList = serializedObject.FindProperty("_keyList");
            _valueList = serializedObject.FindProperty("_valueList");
        }

        public override void OnInspectorGUI()
        {
            _keyDuplicateHashSet.Clear();
            _keyNotFoundHashSet.Clear();
            serializedObject.Update();
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_script);
            }

            while (_valueList.arraySize < _keyList.arraySize)
            {
                _valueList.arraySize++;
            }

            while (_valueList.arraySize > _keyList.arraySize)
            {
                _valueList.arraySize--;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                _keyList.isExpanded = EditorGUILayout.Foldout(_keyList.isExpanded, "Dictionary", true);
                using (new EditorGUI.DisabledScope(_keyList.arraySize == 0))
                {
                    if (GUILayout.Button("Clear", GUILayout.Width(ButtonSize * 3)))
                    {
                        _keyList.ClearArray();
                        _valueList.ClearArray();
                    }

                    if (GUILayout.Button("-", GUILayout.Width(ButtonSize)))
                    {
                        _keyList.arraySize--;
                    }
                }

                if (GUILayout.Button("+", GUILayout.Width(ButtonSize)))
                {
                    _keyList.arraySize++;
                }
            }

            if (_keyList.isExpanded)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    for (var i = 0; i < _keyList.arraySize; i++)
                    {
                        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.PropertyField(_keyList.GetArrayElementAtIndex(i), new GUIContent("Key"));
                                using (new EditorGUI.DisabledScope(i == 0))
                                {
                                    if (GUILayout.Button("↑", GUILayout.Width(ButtonSize)))
                                    {
                                        _keyList.MoveArrayElement(i, i - 1);
                                        _valueList.MoveArrayElement(i, i - 1);
                                    }
                                }

                                using (new EditorGUI.DisabledScope(i == _keyList.arraySize - 1))
                                {
                                    if (GUILayout.Button("↓", GUILayout.Width(ButtonSize)))
                                    {
                                        _keyList.MoveArrayElement(i, i + 1);
                                        _valueList.MoveArrayElement(i, i + 1);
                                    }
                                }

                                if (GUILayout.Button("-", GUILayout.Width(ButtonSize)))
                                {
                                    _keyList.DeleteArrayElementAtIndex(i);
                                    _valueList.DeleteArrayElementAtIndex(i);
                                    break;
                                }

                                if (GUILayout.Button("+", GUILayout.Width(ButtonSize)))
                                {
                                    _keyList.InsertArrayElementAtIndex(i);
                                    _valueList.InsertArrayElementAtIndex(i);
                                }
                            }

                            using (new EditorGUI.IndentLevelScope())
                            {
                                var value = _valueList.GetArrayElementAtIndex(i);
                                EditorGUILayout.PropertyField(value, new GUIContent("Value"));
                            }

                            var key = _keyList.GetArrayElementAtIndex(i).intValue;
                            if (!_keyDuplicateHashSet.Add(key))
                            {
                                EditorGUILayout.HelpBox("Duplicate key", MessageType.Error);
                            }
                        }
                    }
                }
            }

            for (var i = 0; i < _keyList.arraySize; i++)
            {
                _keyNotFoundHashSet.Add(_keyList.GetArrayElementAtIndex(i).intValue);
            }

            var genericType = target.GetType().BaseType?.GetGenericArguments()[0];
            if (genericType != null)
            {
                foreach (var enumValue in Enum.GetValues(genericType))
                {
                    if (_keyNotFoundHashSet.Contains((int)enumValue))
                    {
                        continue;
                    }

                    EditorGUILayout.HelpBox($"{enumValue} is not Registered", MessageType.Error);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
