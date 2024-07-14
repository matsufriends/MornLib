using UnityEngine;

namespace MornUI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class MornUIVisibilityMoveMono : MornUIVisibilityMonoBase
    {
        private const float LerpT = 10;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector2 _showPos;
        [SerializeField] private Vector2 _hidePos;
        private bool _aimChanged;
        private Vector2 _aimPos;

        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!_aimChanged) return;

            _rectTransform.anchoredPosition =
                Vector2.Lerp(_rectTransform.anchoredPosition, _aimPos, Time.deltaTime * LerpT);
            if (Vector2.Distance(_rectTransform.anchoredPosition, _aimPos) < 0.01f)
            {
                _rectTransform.anchoredPosition = _aimPos;
                _aimChanged = false;
            }
        }

        public override void Show(bool immediate = false)
        {
            _aimPos = _showPos;
            _aimChanged = true;
            if (immediate) _rectTransform.anchoredPosition = _showPos;
        }

        public override void Hide(bool immediate = false)
        {
            _aimPos = _hidePos;
            _aimChanged = true;
            if (immediate) _rectTransform.anchoredPosition = _hidePos;
        }
    }
}