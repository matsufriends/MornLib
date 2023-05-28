using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MornSound
{
    [CustomEditor(typeof(MornSoundSolverMonoBase<>), true)]
    internal sealed class MornSoundSolverMonoBaseEditor : Editor
    {
        private SerializedProperty _scriptProperty;
        private SerializedProperty _keyListProperty;
        private SerializedProperty _clipListProperty;
        private SerializedProperty _isRandomPitchListProperty;
        private readonly List<int> _cachedKeyList = new();

        private void OnEnable()
        {
            _scriptProperty = serializedObject.FindProperty("m_Script");
            _keyListProperty = serializedObject.FindProperty("_keyList");
            _clipListProperty = serializedObject.FindProperty("_clipList");
            _isRandomPitchListProperty = serializedObject.FindProperty("_isRandomPitchList");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ShowHeaderProperties();
            AdjustElements();
            if (GUILayout.Button("Add"))
            {
                _keyListProperty.InsertArrayElementAtIndex(_keyListProperty.arraySize);
                _clipListProperty.InsertArrayElementAtIndex(_clipListProperty.arraySize);
                _isRandomPitchListProperty.InsertArrayElementAtIndex(_isRandomPitchListProperty.arraySize);
            }

            _cachedKeyList.Clear();
            for (var i = 0; i < _keyListProperty.arraySize; i++)
            {
                var keyProperty = _keyListProperty.GetArrayElementAtIndex(i);
                var clipProperty = _clipListProperty.GetArrayElementAtIndex(i);
                var isRandomPitchProperty = _isRandomPitchListProperty.GetArrayElementAtIndex(i);
                var width = EditorGUIUtility.currentViewWidth - 40;
                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
                    EditorGUILayout.PropertyField(keyProperty, GUIContent.none, GUILayout.Width(width * 0.2f));
                    if (_cachedKeyList.Contains(keyProperty.enumValueFlag))
                    {
                        EditorGUILayout.HelpBox("Keyが重複しています。", MessageType.Error);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(clipProperty, GUIContent.none, GUILayout.Width(width * 0.3f));
                        EditorGUILayout.LabelField("RandomPitch", GUILayout.Width(width * 0.3f - 20));
                        EditorGUILayout.PropertyField(isRandomPitchProperty, GUIContent.none, GUILayout.Width(20));
                    }

                    if (GUILayout.Button("Delete"))
                    {
                        _keyListProperty.DeleteArrayElementAtIndex(i);
                        _clipListProperty.DeleteArrayElementAtIndex(i);
                        _isRandomPitchListProperty.DeleteArrayElementAtIndex(i);
                        continue;
                    }

                    _cachedKeyList.Add(keyProperty.enumValueFlag);
                }

                GUILayout.Space(10);
            }

            ShowNotContainEnums();
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowHeaderProperties()
        {
            //m_ScriptをDisableで描画
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_scriptProperty);
            }

            //他のプロパティを描画
            var iterator = serializedObject.GetIterator();
            var isEnterChildren = true;
            while (iterator.NextVisible(isEnterChildren))
            {
                isEnterChildren = false;
                if (iterator.name is "m_Script" or "_keyList" or "_clipList" or "_isRandomPitchList")
                {
                    continue;
                }

                EditorGUILayout.PropertyField(iterator, true);
            }
        }

        private void AdjustElements()
        {
            var max = Mathf.Max(_keyListProperty.arraySize, _clipListProperty.arraySize, _isRandomPitchListProperty.arraySize);
            for (var i = 0; i < max; i++)
            {
                if (i >= _keyListProperty.arraySize)
                {
                    _keyListProperty.InsertArrayElementAtIndex(i);
                }

                if (i >= _clipListProperty.arraySize)
                {
                    _clipListProperty.InsertArrayElementAtIndex(i);
                }

                if (i >= _isRandomPitchListProperty.arraySize)
                {
                    _isRandomPitchListProperty.InsertArrayElementAtIndex(i);
                }
            }
        }

        private void ShowNotContainEnums()
        {
            var genericType = target.GetType().BaseType?.GetGenericArguments()[0];
            foreach (var enumValue in Enum.GetValues(genericType))
            {
                if (_cachedKeyList.Contains((int)enumValue))
                {
                    continue;
                }

                EditorGUILayout.HelpBox($"{enumValue}が登録されていません。", MessageType.Error);
            }
        }
    }
}
