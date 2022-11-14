#if USE_TEXTMESHPRO
#if UNITY_EDITOR
using MornLib.Extensions;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine.EventSystems;
#endif
namespace MornLib.Mono {
    public class MornTMPDropdown : TMP_Dropdown {
        public bool IsClickOnMouseRight;
        public bool IsClickOnMouseMiddle;
        public bool IsClickOnMouseLeft;
        public override void OnPointerClick(PointerEventData eventData) {
            if(eventData.IsLeftClick() && IsClickOnMouseLeft) Show();
            if(eventData.IsMiddleClick() && IsClickOnMouseMiddle) Show();
            if(eventData.IsRightClick() && IsClickOnMouseRight) Show();
        }
    }
    #if UNITY_EDITOR
    [CustomEditor(typeof(MornTMPDropdown))]
    public class MornTMPDropdownEditor : DropdownEditor {
        public override void OnInspectorGUI() {
            var dropdown = (MornTMPDropdown)target;
            dropdown.IsClickOnMouseRight  = EditorGUILayout.Toggle("IsClickOnMouseRight",dropdown.IsClickOnMouseRight);
            dropdown.IsClickOnMouseMiddle = EditorGUILayout.Toggle("IsClickOnMouseMiddle",dropdown.IsClickOnMouseMiddle);
            dropdown.IsClickOnMouseLeft   = EditorGUILayout.Toggle("IsClickOnMouseLeft",dropdown.IsClickOnMouseLeft);
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
    #endif
}
#endif