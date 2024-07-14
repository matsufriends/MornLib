using System.Linq;
using MornLib.Extensions;
using UnityEditor;
using UnityEngine;

namespace MornLib.Editor
{
    public class MornCanvasSortWindow : EditorWindow
    {
        private Vector2 _scrollPos;

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            {
                GUILayout.Label("Canvas Sorting Order");
                var list = FindObjectsOfType<Canvas>().OrderBy(x => x.sortingOrder);
                foreach (var canvas in list) GUILayout.Label($"{canvas.sortingOrder}:{canvas.transform.GetPath()}");
            }
            EditorGUILayout.EndScrollView();
        }

        [MenuItem("MornLib/" + nameof(MornCanvasSortWindow))]
        private static void Open()
        {
            var window = GetWindow<MornCanvasSortWindow>();
            window.titleContent = new GUIContent(nameof(MornCanvasSortWindow));
        }
    }
}