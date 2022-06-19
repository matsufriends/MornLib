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
            DrawTag(selectionRect,gameObject);
        }
        private static void DrawColor(Rect selectionRect,GameObject gameObject) {
            if(gameObject.TryGetComponent<HierarchyEx>(out var ownColor)) {
                EditorGUI.DrawRect(selectionRect,ownColor.BackColor);
                return;
            }
            var parentColors = gameObject.GetComponentsInParent<HierarchyEx>();
            if(parentColors == null) return;
            foreach(var parent in parentColors) {
                if(parent.ApplyChildren == false) continue;
                var k = GetKRecursion(parent.transform,gameObject.transform);
                EditorGUI.DrawRect(selectionRect,parent.BackColor * k);
                return;
            }
        }
        private static float GetKRecursion(Transform aim,Transform own) {
            if(aim == own) return 1;
            var pare = own.parent;
            return Mathf.InverseLerp(pare.childCount + 4,-1,own.GetSiblingIndex()) * GetKRecursion(aim,pare);
        }
        private static void DrawTag(Rect selectionRect,GameObject gameObject) {
            var tag = gameObject.tag;
            var style = new GUIStyle();
            if(tag == "Line") {
                selectionRect.xMin = 32;
                EditorGUI.DrawRect(selectionRect,GetBackGroundColor(selectionRect));
                style.normal.textColor = new Color32(255,255,255,32);
                style.alignment        = TextAnchor.MiddleCenter;
                EditorGUI.LabelField(selectionRect,$"-----{gameObject.name}-----",style);
            } else {
                selectionRect.xMin     += selectionRect.width - 80;
                selectionRect.xMax     -= 16;
                style.normal.textColor =  tag == "Untagged" ? Color.red : GUI.color;
                style.alignment        =  TextAnchor.MiddleRight;
                EditorGUI.LabelField(selectionRect,tag,style);
            }
        }
        private static Color GetBackGroundColor(Rect rect) {
            return rect.Contains(Event.current.mousePosition) ? new Color32(68,68,68,255) : new Color32(56,56,56,255);
        }
    }
}