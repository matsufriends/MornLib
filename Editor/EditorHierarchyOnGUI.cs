using MornLib.Mono;
using UnityEditor;
using UnityEngine;
namespace MornLib.Editor {
    public static class EditorHierarchyOnGUI {
        [InitializeOnLoadMethod]
        private static void AddHierarchyItemOnGUI() {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }
        private static void HierarchyWindowItemOnGUI(int instanceID,Rect selectionRect) {
            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if(gameObject == null) return;
            DrawColor(selectionRect,gameObject);
            DrawLabel(selectionRect,gameObject);
            DrawTag(instanceID,selectionRect,gameObject);
        }
        private static void DrawColor(Rect selectionRect,GameObject gameObject) {
            var hasDrawn = false;
            if(gameObject.TryGetComponent<MornHierarchy>(out var ownColor)) {
                DrawTransparentRect(selectionRect,ownColor.BackColor);
                hasDrawn = true;
            }
            var mornHiArray = gameObject.GetComponentsInParent<MornHierarchy>();
            if(mornHiArray == null) return;
            var target = gameObject.transform;
            var depth = 0;
            for(var hiIndex = 0;hiIndex < mornHiArray.Length;) {
                var hi = mornHiArray[hiIndex];
                if(target == hi.transform) {
                    hiIndex++;
                    continue;
                }
                if(hi.ApplyChildren == false) {
                    hiIndex++;
                    target = target.parent;
                    depth++;
                    hasDrawn = true;
                    continue;
                }

                //Side
                var kBack = GetKRecursion(hi.transform,target.parent);
                var rectA = selectionRect;
                rectA.xMax = rectA.xMin - 14 * (depth);
                rectA.xMin = rectA.xMax - 14;
                DrawTransparentRect(rectA,hi.BackColor * kBack);

                //Main
                if(hasDrawn == false) {
                    var kFront = GetKRecursion(hi.transform,gameObject.transform);
                    DrawTransparentRect(selectionRect,hi.BackColor * kFront);
                    hasDrawn = true;
                }
                target = target.parent;
                depth++;
            }
        }
        private static void DrawTransparentRect(Rect rect,Color color) {
            color.a = 0.3f;
            EditorGUI.DrawRect(rect,color);
        }
        private static float GetKRecursion(Transform aim,Transform own) {
            if(aim == own) return 1f;
            const int offset = 2;
            var pare = own.parent;
            return Mathf.InverseLerp(pare.childCount + offset,-1,own.GetSiblingIndex()) * GetKRecursion(aim,pare);
        }
        private static void DrawLabel(Rect selectionRect,GameObject gameObject) {
            selectionRect.xMin += 18;
            var style = new GUIStyle();
            style.normal.textColor = GUI.contentColor;
            style.alignment        = TextAnchor.UpperLeft;
            EditorGUI.LabelField(selectionRect,gameObject.name,style);
        }
        private static void DrawTag(int instanceId,Rect selectionRect,GameObject gameObject) {
            var tag = gameObject.tag;
            if(tag == "Line") DrawLine(instanceId,selectionRect,gameObject);
            var style = new GUIStyle();
            selectionRect.xMax     -= 16;
            selectionRect.xMin     += selectionRect.width - 80;
            style.normal.textColor =  tag == "Untagged" ? Color.red : GUI.color;
            style.alignment        =  TextAnchor.MiddleRight;
            EditorGUI.LabelField(selectionRect,tag,style);
        }
        private static void DrawLine(int instanceID,Rect selectionRect,GameObject gameObject) {
            //DrawBack
            selectionRect.xMin = 32;
            EditorGUI.DrawRect(selectionRect,GetBackGroundColor(instanceID,selectionRect));

            //UpperLine
            var upperLineRect = selectionRect;
            upperLineRect.yMax = upperLineRect.yMin + 2;
            EditorGUI.DrawRect(upperLineRect,Color.black);

            //LowerLine
            var lowerLineRect = selectionRect;
            lowerLineRect.yMin = lowerLineRect.yMax - 2;
            EditorGUI.DrawRect(lowerLineRect,Color.black);

            //Label
            var style = new GUIStyle();
            style.normal.textColor = GUI.contentColor;
            style.alignment        = TextAnchor.UpperCenter;
            EditorGUI.LabelField(selectionRect,gameObject.name,style);
        }
        private static Color GetBackGroundColor(int instanceID,Rect rect) {
            if(Selection.Contains(instanceID)) return new Color32(44,93,134,255);
            return rect.Contains(Event.current.mousePosition) ? new Color32(68,68,68,255) : new Color32(56,56,56,255);
        }
    }
}