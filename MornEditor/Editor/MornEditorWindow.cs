using UnityEngine;
using UnityEditor;

namespace MornEditor
{
    public sealed class MornEditorWindow : EditorWindow
    {
        private Vector2 _scroll;

        [MenuItem("MornLib/MornEditorWindow")]
        private static void ShowWindow()
        {
            GetWindow<MornEditorWindow>();
        }

        private void OnGUI()
        {
            MornEditorCore.CheckDisposed();
            using (var scrollScope = new EditorGUILayout.ScrollViewScope(_scroll))
            {
                foreach (var info in MornEditorCore.OnGUIHashSet)
                {
                    GUILayout.Label(info.Label);
                    info.OnGUI();
                    GUILayout.Space(10);
                }

                _scroll = scrollScope.scrollPosition;
            }
        }
    }
}