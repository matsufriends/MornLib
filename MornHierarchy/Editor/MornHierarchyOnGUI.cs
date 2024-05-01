using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace MornHierarchy
{
    internal static class MornHierarchyOnGUI
    {
        private static readonly Color s_normalBackColor = new Color32(56, 56, 56, 255);
        private static readonly Color s_highlightedBackColor = new Color32(68, 68, 68, 255);
        private static readonly Color s_selectedBackColor = new Color32(44, 93, 134, 255);

        [InitializeOnLoadMethod]
        private static void AddHierarchyItemOnGUI()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        private static void HierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (gameObject == null)
            {
                return;
            }

            DrawColor(selectionRect, gameObject);
            if (gameObject.TryGetComponent<MornHierarchyLine>(out _))
            {
                DrawLine(selectionRect, gameObject);
            }

            var sortingGroup = gameObject.GetComponentInParent<SortingGroup>(true);
            if (sortingGroup != null)
            {
                DrawSpriteSorting(selectionRect, sortingGroup.sortingLayerName, sortingGroup.sortingOrder);
            }
            else
            {
                var renderer = gameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    DrawSpriteSorting(selectionRect, renderer.sortingLayerName, renderer.sortingOrder);
                }
            }

            if (MornHierarchySettings.instance.ShowTag)
            {
                DrawTag(selectionRect, gameObject);
            }
        }

        private static void DrawColor(Rect selectionRect, GameObject gameObject)
        {
            var hasDrawn = false;
            if (gameObject.TryGetComponent<MornHierarchyColor>(out var ownColor))
            {
                DrawTransparentRect(selectionRect, ownColor.BackColor);
                hasDrawn = true;
            }

            var colorHierarchy = gameObject.GetComponentsInParent<MornHierarchyColor>(true);
            if (colorHierarchy == null)
            {
                return;
            }

            //targetが一番親のcolorのtransform一致するまで
            //親を順に辿りながら色を塗る
            var target = gameObject.transform;
            var drawOffset = 0;
            for (var colorIndex = 0; colorIndex < colorHierarchy.Length;)
            {
                var drawColor = colorHierarchy[colorIndex];
                if (target == drawColor.transform)
                {
                    colorIndex++;
                    continue;
                }

                if (drawColor.ApplyChildren == false)
                {
                    colorIndex++;
                    target = target.parent;
                    drawOffset++;
                    hasDrawn = true;
                    continue;
                }

                //Side
                var colorRect = selectionRect;
                colorRect.xMax = colorRect.xMin - 14 * drawOffset;
                colorRect.xMin = colorRect.xMax - 14;
                DrawTransparentRect(colorRect, drawColor.BackColor * GetTransformDepth(drawColor.transform, target.parent));

                //Main
                if (hasDrawn == false)
                {
                    DrawTransparentRect(selectionRect, drawColor.BackColor * GetTransformDepth(drawColor.transform, gameObject.transform));
                    hasDrawn = true;
                }

                target = target.parent;
                drawOffset++;
            }
        }

        private static void DrawSpriteSorting(Rect selectionRect, string sortingLayerName, int sortingOrder)
        {
            // SortingLayers
            var style = new GUIStyle();
            selectionRect.xMax -= 16;
            selectionRect.xMin += selectionRect.width - 80;
            style.normal.textColor = GUI.contentColor;
            style.alignment = TextAnchor.MiddleRight;
            EditorGUI.LabelField(selectionRect, $"{sortingLayerName}-{sortingOrder:00}", style);
        }

        private static void DrawTag(Rect selectionRect, GameObject gameObject)
        {
            var tag = gameObject.tag;
            var style = new GUIStyle();
            selectionRect.xMax -= 16;
            selectionRect.xMin += selectionRect.width - 80;
            style.normal.textColor = tag == "Untagged" ? Color.red : GUI.color;
            style.alignment = TextAnchor.MiddleRight;
            EditorGUI.LabelField(selectionRect, tag, style);
        }

        private static void DrawLine(Rect selectionRect, GameObject gameObject)
        {
            //DrawBack
            EditorGUI.DrawRect(selectionRect, GetBackGroundColor(gameObject.GetInstanceID(), selectionRect));
            selectionRect.xMin = 32;

            //UpperLine
            var upperLineRect = selectionRect;
            upperLineRect.yMax = upperLineRect.yMin + 2;
            EditorGUI.DrawRect(upperLineRect, Color.black);

            //LowerLine
            var lowerLineRect = selectionRect;
            lowerLineRect.yMin = lowerLineRect.yMax - 2;
            EditorGUI.DrawRect(lowerLineRect, Color.black);

            //Label
            var style = new GUIStyle();
            style.normal.textColor = GUI.contentColor;
            style.alignment = TextAnchor.UpperCenter;
            EditorGUI.LabelField(selectionRect, gameObject.name, style);
        }

        private static void DrawTransparentRect(Rect rect, Color color)
        {
            color.a = MornHierarchySettings.instance.Transparent;
            EditorGUI.DrawRect(rect, color);
        }

        private static float GetTransformDepth(Transform home, Transform own)
        {
            if (home == own)
            {
                return 1f;
            }

            var pare = own.parent;
            return Mathf.InverseLerp(pare.childCount + 2, -1, own.GetSiblingIndex()) * GetTransformDepth(home, pare);
        }

        private static Color GetBackGroundColor(int instanceID, Rect rect)
        {
            if (Selection.Contains(instanceID))
            {
                return s_selectedBackColor;
            }

            return rect.Contains(Event.current.mousePosition) ? s_highlightedBackColor : s_normalBackColor;
        }
    }
}