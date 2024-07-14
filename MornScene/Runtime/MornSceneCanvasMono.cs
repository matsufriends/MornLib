using UnityEngine;
using UnityEngine.UI;

namespace MornScene
{
    [RequireComponent(typeof(CanvasGroup), typeof(CanvasScaler))]
    public sealed class MornSceneCanvasMono : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Reset()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        internal void SetActiveImmediate(bool isActive)
        {
            _canvas.enabled = isActive;
            _canvasGroup.alpha = isActive ? 1 : 0;
            _canvasGroup.interactable = isActive;
            _canvasGroup.blocksRaycasts = isActive;
        }
    }
}