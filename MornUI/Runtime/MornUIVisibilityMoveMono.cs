using UnityEngine;

namespace MornUI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class MornUIVisibilityMoveMono : MornUIVisibilityMonoBase
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector2 _showPos;
        [SerializeField] private Vector2 _hidePos;
        private Vector2 _aimPos;
        private const float LerpT = 10;

        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OverridePositions(Vector2 showPos, Vector2 hidePos)
        {
            _showPos = showPos;
            _hidePos = hidePos;
        }

        public override void Show(bool immediate = false)
        {
            _aimPos = _showPos;
            if (immediate)
            {
                _rectTransform.anchoredPosition = _showPos;
            }
        }

        public override void Hide(bool immediate = false)
        {
            _aimPos = _hidePos;
            if (immediate)
            {
                _rectTransform.anchoredPosition = _hidePos;
            }
        }

        private void Update()
        {
            _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition, _aimPos, Time.deltaTime * LerpT);
        }
    }
}