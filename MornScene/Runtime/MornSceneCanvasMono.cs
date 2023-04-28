using UnityEngine;
using UnityEngine.UI;

namespace MornScene
{
    [RequireComponent(typeof(CanvasGroup), typeof(CanvasScaler))]
    public sealed class MornSceneCanvasMono : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasScaler _canvasScaler;

        internal void ApplyCanvasScale(int width, int height)
        {
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(width, height);
            _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
        }

        internal void SetActiveImmediate(bool isActive)
        {
            _canvas.enabled = isActive;
        }

        private void Reset()
        {
            _canvas = GetComponent<Canvas>();
            _canvasScaler = GetComponent<CanvasScaler>();
        }
    }
}
