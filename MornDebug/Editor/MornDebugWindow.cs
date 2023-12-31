using UnityEditor;
using UnityEngine;

namespace MornDebug
{
    public sealed class MornDebugWindow : EditorWindow
    {
        private Vector2 _scroll;

        [MenuItem("MornLib/" + nameof(MornDebugWindow))]
        private static void ShowWindow()
        {
            GetWindow<MornDebugWindow>();
        }

        private void OnGUI()
        {
            MornDebugCore.CheckDisposed();
            using (var scrollScope = new EditorGUILayout.ScrollViewScope(_scroll))
            {
                foreach (var info in MornDebugCore.OnGUIHashSet)
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