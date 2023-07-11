using UnityEditor;
using UnityEngine;

namespace MornScene
{
    [CustomEditor(typeof(MornSceneSolver))]
    public sealed class MornSceneSolverBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var dictionaryProperty = serializedObject.FindProperty("_sceneDictionary");
            if (dictionaryProperty.objectReferenceValue == null)
            {
                return;
            }

            var dictionary = (MornSceneDictionaryMono)dictionaryProperty.objectReferenceValue;
            if (GUILayout.Button("HideAll"))
            {
                ChangeScene(dictionary, null);
            }

            foreach (var pair in dictionary.GetDictionary())
            {
                if (GUILayout.Button($"[{pair.Key.SceneName}]"))
                {
                    ChangeScene(dictionary, pair.Key);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private static void ChangeScene(MornSceneDictionaryMono sceneDictionary, MornSceneDataSo sceneData)
        {
            foreach (var pair in sceneDictionary.GetDictionary())
            {
                pair.Value.SetSceneActive(pair.Key, sceneData != null && pair.Key == sceneData);
                EditorUtility.SetDirty(pair.Value);
            }

            if (sceneData == null)
            {
                Debug.Log("Hide All Scenes.");
            }
            else
            {
                Debug.Log($"Scene Changed to {sceneData.SceneName}.");
            }
        }
    }
}
