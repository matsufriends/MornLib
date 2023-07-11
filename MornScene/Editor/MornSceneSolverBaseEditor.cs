namespace MornScene
{
    /*
    [CustomEditor(typeof(MornSceneSolver), true)]
    public sealed class MornSceneSolverBaseEditor : Editor
    {
        private SerializedProperty _scriptProperty;
        private SerializedProperty _firstSceneProperty;

        private void OnEnable()
        {
            _scriptProperty = serializedObject.FindProperty("m_Script");
            _firstSceneProperty = serializedObject.FindProperty("_firstSceneType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ShowHeaderProperties();
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
                method?.Invoke(
                    target,
                    new object[]
                    {
                    });
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_firstSceneProperty);
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
                    method?.Invoke(
                        target,
                        new object[]
                        {
                            sceneName,
                        });
                }
            }
        }
    }
    */
}
