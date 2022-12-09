using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornLib.Mono
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MornCanvasGroupSetterMono : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public void SetActiveImmediate(bool isActive)
        {
            _canvasGroup.alpha = isActive ? 1 : 0;
            _canvasGroup.blocksRaycasts = isActive;
        }

        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MornCanvasGroupSetterMono))]
    public class MornCanvasGroupSetterEditor : Editor
    {
        private MornCanvasGroupSetterMono _canvasGroupSetter;

        private void OnEnable()
        {
            _canvasGroupSetter = (MornCanvasGroupSetterMono)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("SetActive"))
            {
                _canvasGroupSetter.SetActiveImmediate(true);
            }

            if (GUILayout.Button("SetInactive"))
            {
                _canvasGroupSetter.SetActiveImmediate(false);
            }
        }
    }
#endif
}
