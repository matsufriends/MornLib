using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MornScene
{
    [CustomEditor(typeof(MornSceneSolverBase<>), true)]
    public sealed class MornSceneSolverBaseEditor : Editor
    {
        private SerializedProperty _scriptProperty;
        private SerializedProperty _widthProperty;
        private SerializedProperty _heightProperty;
        private SerializedProperty _firstSceneProperty;
        private SerializedProperty _keyListProperty;
        private SerializedProperty _valueList;
        private readonly List<int> _cachedKeyList = new();

        private void OnEnable()
        {
            _scriptProperty = serializedObject.FindProperty("m_Script");
            _widthProperty = serializedObject.FindProperty("_width");
            _heightProperty = serializedObject.FindProperty("_height");
            _firstSceneProperty = serializedObject.FindProperty("_firstSceneType");
            _keyListProperty = serializedObject.FindProperty("_keyList");
            _valueList = serializedObject.FindProperty("_valueList");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ShowHeaderProperties();
            AdjustElements();
            if (GUILayout.Button("Add"))
            {
                _keyListProperty.InsertArrayElementAtIndex(_keyListProperty.arraySize);
                _valueList.InsertArrayElementAtIndex(_valueList.arraySize);
            }

            _cachedKeyList.Clear();
            for (var i = 0; i < _keyListProperty.arraySize; i++)
            {
                var keyProperty = _keyListProperty.GetArrayElementAtIndex(i);
                var clipProperty = _valueList.GetArrayElementAtIndex(i);
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
                    }

                    if (GUILayout.Button("Delete"))
                    {
                        _keyListProperty.DeleteArrayElementAtIndex(i);
                        _valueList.DeleteArrayElementAtIndex(i);
                        continue;
                    }

                    _cachedKeyList.Add(keyProperty.enumValueFlag);
                }

                GUILayout.Space(10);
            }

            ShowNotContainEnums();
            ShowDebugButton();
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowHeaderProperties()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_scriptProperty);
            }

            EditorGUILayout.PropertyField(_widthProperty);
            EditorGUILayout.PropertyField(_heightProperty);
            if (GUILayout.Button("ApplyCanvasScale"))
            {
                var method = target.GetType().GetMethod("ApplyCanvasScale");
                method?.Invoke(target, new object[] { });
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_firstSceneProperty);
        }

        private void AdjustElements()
        {
            var max = Mathf.Max(_keyListProperty.arraySize, _valueList.arraySize);
            for (var i = 0; i < max; i++)
            {
                if (i >= _keyListProperty.arraySize)
                {
                    _keyListProperty.InsertArrayElementAtIndex(i);
                }

                if (i >= _valueList.arraySize)
                {
                    _valueList.InsertArrayElementAtIndex(i);
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

        private void ShowDebugButton()
        {
            var targetType = target.GetType();
            var genericType = targetType.BaseType?.GetGenericArguments()[0];
            if (genericType == null)
            {
                return;
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("HideAll"))
            {
                var method = targetType.GetMethod("HideAll");
                method?.Invoke(target, null);
            }

            foreach (var sceneName in Enum.GetNames(genericType))
            {
                if (GUILayout.Button($"[{sceneName}]"))
                {
                    var method = targetType.GetMethod("ChangeScene");
                    method?.Invoke(target, new object[] { sceneName });
                }
            }
        }
    }
}
