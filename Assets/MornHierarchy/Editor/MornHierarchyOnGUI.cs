using UnityEditor;
using UnityEngine;

namespace MornHierarchy
{
    /// <summary>Hierarchyの表示を書き換えるEditor拡張</summary>
    public static class MornHierarchyOnGUI
    {
        /// <summary>通常の背景色</summary>
        private static readonly Color s_normalBackColor = new Color32(56, 56, 56, 255);

        /// <summary>オンカーソル中の背景色</summary>
        private static readonly Color s_highlightedBackColor = new Color32(68, 68, 68, 255);

        /// <summary>選択中の背景色</summary>
        private static readonly Color s_selectedBackColor = new Color32(44, 93, 134, 255);

        /// <summary>Hierarchy描画処理の拡張</summary>
        [InitializeOnLoadMethod]
        private static void AddHierarchyItemOnGUI()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        /// <inheritdoc cref="EditorApplication.HierarchyWindowItemCallback"/>
        private static void HierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (gameObject == null)
            {
                return;
            }

            DrawColor(selectionRect, gameObject);
            DrawLabel(selectionRect, gameObject);
            if (gameObject.TryGetComponent<MornHierarchyLine>(out _))
            {
                DrawLine(selectionRect, gameObject);
            }

            if (MornHierarchySettings.instance.ShowTag)
            {
                DrawTag(selectionRect, gameObject);
            }
        }

        /// <summary>背景色の描画</summary>
        /// <param name="selectionRect">描画範囲</param>
        /// <param name="gameObject">描画するGameObject</param>
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

        /// <summary>項目名の描画</summary>
        /// <param name="selectionRect">描画範囲</param>
        /// <param name="gameObject">描画するGameObject</param>
        private static void DrawLabel(Rect selectionRect, GameObject gameObject)
        {
            selectionRect.xMin += 18;
            var style = new GUIStyle();
            style.normal.textColor = gameObject.activeInHierarchy ? GUI.contentColor : Color.gray;
            style.alignment = TextAnchor.UpperLeft;
            EditorGUI.LabelField(selectionRect, gameObject.name, style);
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

        /// <summary>区切り線の描画</summary>
        /// <param name="selectionRect">描画範囲</param>
        /// <param name="gameObject">描画するGameObject</param>
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

        /// <summary>半透明の矩形を描画する</summary>
        /// <param name="rect">描画範囲</param>
        /// <param name="color">描画色</param>
        private static void DrawTransparentRect(Rect rect, Color color)
        {
            color.a = MornHierarchySettings.instance.Transparent;
            EditorGUI.DrawRect(rect, color);
        }

        /// <summary>Transformからの深さを求め、0~1で返す</summary>
        /// <param name="home">基準のTransform</param>
        /// <param name="own">自身のTransform</param>
        /// <returns>深さを0~1で返す</returns>
        private static float GetTransformDepth(Transform home, Transform own)
        {
            if (home == own)
            {
                return 1f;
            }

            var pare = own.parent;
            return Mathf.InverseLerp(pare.childCount + 2, -1, own.GetSiblingIndex()) * GetTransformDepth(home, pare);
        }

        /// <summary>Hierarchyの背景色を返す</summary>
        /// <param name="instanceID">描画するGameObjectのInstanceId</param>
        /// <param name="rect">描画範囲</param>
        /// <returns>背景色</returns>
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
