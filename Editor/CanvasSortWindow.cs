using System.Linq;
using UnityEditor;
using UnityEngine;
namespace MornLib.Editor {
    public class CanvasSortWindow : EditorWindow {
        private Vector2 _scrollPos;
        [MenuItem("Window/Morn/" + nameof(CanvasSortWindow))]
        private static void Open() {
            var window = GetWindow<CanvasSortWindow>();
            window.titleContent = new GUIContent(nameof(CanvasSortWindow));
        }
        private void OnGUI() {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            {
                GUILayout.Label("Canvas Sorting Order");
                var list = FindObjectsOfType<Canvas>().OrderBy(x => x.sortingOrder);
                foreach(var canvas in list) {
                    GUILayout.Label($"{canvas.sortingOrder}:{canvas.gameObject.name}");
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}