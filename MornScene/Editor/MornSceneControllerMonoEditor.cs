using System;
using UnityEditor;
using UnityEngine;

namespace MornScene
{
    [CustomEditor(typeof(MornSceneControllerMonoBase<>), true)]
    public sealed class MornSceneControllerMonoEditor : Editor
    {
        private SerializedProperty _sceneListProperty;

        private void OnEnable()
        {
            _sceneListProperty = serializedObject.FindProperty("_sceneList");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var targetType = target.GetType();
            var genericType = targetType.BaseType?.GetGenericArguments()[0];
            if (genericType == null)
            {
                return;
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("FindScenesInEditor"))
            {
                var method = targetType.GetMethod("FindScenesInEditor");
                method?.Invoke(target, new object[] { });
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("ApplyCanvasScale"))
            {
                var method = targetType.GetMethod("ApplyCanvasScaleInEditor");
                method?.Invoke(target, new object[] { });
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("HideAll"))
            {
                var method = targetType.GetMethod("ChangeSceneInEditor");
                method?.Invoke(target, new object[] { "" });
            }

            foreach (var sceneName in Enum.GetNames(genericType))
            {
                if (GUILayout.Button($"[{sceneName}]"))
                {
                    var method = targetType.GetMethod("ChangeSceneInEditor");
                    method?.Invoke(target, new object[] { sceneName });
                }
            }
        }
    }
}
