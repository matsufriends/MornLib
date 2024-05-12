using UnityEngine;

namespace MornUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class MornUIVisibilityFadeMono : MornUIVisibilityMonoBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        private bool _aimChanged;
        private float _aimAlpha;
        private const float LerpT = 10;

        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Show(bool immediate = false)
        {
            _aimAlpha = 1;
            _aimChanged = true;
            if (immediate)
            {
                _canvasGroup.alpha = 1;
            }
        }

        public override void Hide(bool immediate = false)
        {
            _aimAlpha = 0;
            _aimChanged = true;
            if (immediate)
            {
                _canvasGroup.alpha = 0;
            }
        }

        private void Update()
        {
            if (!_aimChanged)
            {
                return;
            }

            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _aimAlpha, Time.deltaTime * LerpT);
        }
    }
}