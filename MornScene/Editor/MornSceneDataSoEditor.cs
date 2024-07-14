using System.IO;
using UnityEditor;
using UnityEngine;

namespace MornScene
{
    [CustomEditor(typeof(MornSceneDataSo))]
    [CanEditMultipleObjects]
    internal sealed class MornSceneDataSoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("File Name -> Scene Name"))
                foreach (var target in targets)
                {
                    var sceneDataSo = (MornSceneDataSo)target;
                    var assetPath = AssetDatabase.GetAssetPath(sceneDataSo);
                    var fileName = Path.GetFileNameWithoutExtension(assetPath);
                    sceneDataSo.SetSceneName(fileName);
                    EditorUtility.SetDirty(sceneDataSo);
                }
        }
    }
}