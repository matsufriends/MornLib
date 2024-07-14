using UnityEngine;

namespace MornUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class MornUIVisibilityFadeMono : MornUIVisibilityMonoBase
    {
        private const float LerpT = 10;
        [SerializeField] private CanvasGroup _canvasGroup;
        private float _aimAlpha;
        private bool _aimChanged;

        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (!_aimChanged) return;

            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _aimAlpha, Time.deltaTime * LerpT);
            if (Mathf.Abs(_canvasGroup.alpha - _aimAlpha) < 0.01f)
            {
                _canvasGroup.alpha = _aimAlpha;
                _aimChanged = false;
            }
        }

        public override void Show(bool immediate = false)
        {
            _aimAlpha = 1;
            _aimChanged = true;
            if (immediate) _canvasGroup.alpha = 1;
        }

        public override void Hide(bool immediate = false)
        {
            _aimAlpha = 0;
            _aimChanged = true;
            if (immediate) _canvasGroup.alpha = 0;
        }
    }
}