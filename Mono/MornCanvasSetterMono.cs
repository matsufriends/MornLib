using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornLib.Mono
{
    [RequireComponent(typeof(CanvasGroup), typeof(CanvasScaler))]
    public class MornCanvasSetterMono : MonoBehaviour
    {
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            var so = MornCanvasGroupSo.Instance;
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(so.Width, so.Height);
            _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
        }

        public void SetActiveImmediate(bool isActive)
        {
            _canvasGroup.alpha = isActive ? 1 : 0;
            _canvasGroup.blocksRaycasts = isActive;
            gameObject.name = $"Canvas({isActive})";
        }

        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasScaler = GetComponent<CanvasScaler>();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MornCanvasSetterMono))]
    public class MornCanvasGroupSetterEditor : Editor
    {
        private MornCanvasSetterMono _canvasSetter;

        private void OnEnable()
        {
            _canvasSetter = (MornCanvasSetterMono)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Init"))
            {
                _canvasSetter.Init();
                EditorUtility.SetDirty(_canvasSetter);
            }

            if (GUILayout.Button("SetActive"))
            {
                _canvasSetter.SetActiveImmediate(true);
                EditorUtility.SetDirty(_canvasSetter);
            }

            if (GUILayout.Button("SetInactive"))
            {
                _canvasSetter.SetActiveImmediate(false);
                EditorUtility.SetDirty(_canvasSetter);
            }
        }
    }
#endif
}
