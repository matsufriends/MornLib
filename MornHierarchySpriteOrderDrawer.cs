using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace MornLib
{
    public static class MornHierarchySpriteOrderDrawer
    {
        [InitializeOnLoadMethod]
        private static void AddHierarchyItemOnGUI()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject == null) return;
            DrawTag(selectionRect, gameObject);
        }

        private static void DrawTag(Rect selectionRect, GameObject gameObject)
        {
            var sortingGroup = gameObject.GetComponentInParent<SortingGroup>(true);
            if (sortingGroup == null)
            {
                sortingGroup = gameObject.GetComponent<SortingGroup>();
            }

            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer == null)
            {
                return;
            }

            var rendererText = $"{LayerNameToNumber(renderer.sortingLayerName)}-{renderer.sortingOrder}";
            var text = "";
            if (sortingGroup != null)
            {
                text = $"{LayerNameToNumber(sortingGroup.sortingLayerName)}-{sortingGroup.sortingOrder}({rendererText})";
            }
            else
            {
                text = rendererText;
            }

            var style = new GUIStyle();
            selectionRect.xMax -= 16;
            selectionRect.xMin = selectionRect.xMax - 80;
            style.normal.textColor = GUI.color;
            style.alignment = TextAnchor.MiddleRight;
            EditorGUI.LabelField(selectionRect, text, style);
        }

        private static string LayerNameToNumber(string layerName)
        {
            var numberCount = 0;
            for (var i = layerName.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(layerName[i]))
                {
                    break;
                }

                numberCount++;
            }

            return layerName[^numberCount..];
        }
    }
}