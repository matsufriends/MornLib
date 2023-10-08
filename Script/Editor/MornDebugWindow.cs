using MornLib.Debugs;
using UnityEditor;
using UnityEngine;

namespace MornLib.Editor
{
    public class MornDebugWindow : EditorWindow
    {
        private Vector2 _scrollPos;

        [MenuItem("MornLib/" + nameof(MornDebugWindow))]
        private static void Open()
        {
            var window = GetWindow<MornDebugWindow>();
            window.titleContent = new GUIContent(nameof(MornDebugWindow));
        }

        private void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            var text = MornDebugLog.Instance.GetLog();
            if (text.Length > 0)
            {
                GUILayout.Label(text);
            }
            else
            {
                GUILayout.Label("データが登録されていません");
            }
        }
    }
}