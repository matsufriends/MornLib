using UnityEngine;
using UnityEngine.UI;

namespace MornScene
{
    [RequireComponent(typeof(CanvasGroup), typeof(CanvasScaler))]
    public sealed class MornSceneCanvasMono : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasScaler _canvasScaler;

        internal void ApplyCanvasScale(int width, int height)
        {
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(width, height);
            _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
        }

        internal void SetActiveImmediate(bool isActive)
        {
            _canvasGroup.alpha = isActive ? 1 : 0;
            _canvasGroup.interactable = isActive;
            _canvasGroup.blocksRaycasts = isActive;
            //_canvas.enabled = isActive;
        }

        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasScaler = GetComponent<CanvasScaler>();
        }
    }
}
