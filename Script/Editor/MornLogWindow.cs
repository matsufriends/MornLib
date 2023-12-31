using MornLib.Debugs;
using UnityEditor;
using UnityEngine;

namespace MornLib.Editor
{
    public class MornLogWindow : EditorWindow
    {
        private Vector2 _scrollPos;

        [MenuItem("MornLib/" + nameof(MornLogWindow))]
        private static void Open()
        {
            var window = GetWindow<MornLogWindow>();
            window.titleContent = new GUIContent(nameof(MornLogWindow));
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