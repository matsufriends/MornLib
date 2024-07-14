using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MornDictionary
{
    public abstract class MornDictionaryBaseEditor<TKey> : Editor
    {
        private const int ButtonSize = 20;
        private readonly HashSet<TKey> _keyDuplicateHashSet = new();
        protected readonly HashSet<TKey> KeyNotFoundHashSet = new();
        private SerializedProperty _keyList;
        private SerializedProperty _script;
        private SerializedProperty _valueList;

        private void OnEnable()
        {
            _script = serializedObject.FindProperty("m_Script");
            _keyList = serializedObject.FindProperty("_keyList");
            _valueList = serializedObject.FindProperty("_valueList");
        }

        protected abstract TKey GetValue(SerializedProperty property);
        protected abstract void AfterRenderDictionary();

        public override void OnInspectorGUI()
        {
            _keyDuplicateHashSet.Clear();
            KeyNotFoundHashSet.Clear();
            serializedObject.Update();
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_script);
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

                    if (GUILayout.Button("-", GUILayout.Width(ButtonSize))) _keyList.arraySize--;
                }

                if (GUILayout.Button("+", GUILayout.Width(ButtonSize))) _keyList.arraySize++;
            }

            while (_valueList.arraySize < _keyList.arraySize) _valueList.arraySize++;

            while (_valueList.arraySize > _keyList.arraySize) _valueList.arraySize--;

            if (_keyList.isExpanded)
                using (new EditorGUI.IndentLevelScope())
                {
                    for (var i = 0; i < _keyList.arraySize; i++)
                        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.PropertyField(_keyList.GetArrayElementAtIndex(i),
                                    new GUIContent("Key"));
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

                            var key = GetValue(_keyList.GetArrayElementAtIndex(i));
                            if (!_keyDuplicateHashSet.Add(key))
                                EditorGUILayout.HelpBox("Duplicate key", MessageType.Error);
                        }
                }

            for (var i = 0; i < _keyList.arraySize; i++)
            {
                var key = GetValue(_keyList.GetArrayElementAtIndex(i));
                KeyNotFoundHashSet.Add(key);
            }

            AfterRenderDictionary();
            if (GUILayout.Button("Reset Cache")) ((MornDictionaryBaseInternalBase)target).ResetDictionary();

            serializedObject.ApplyModifiedProperties();
        }
    }
}