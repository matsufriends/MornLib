using UnityEngine;


namespace MornCamera
{
    [RequireComponent(typeof(Camera))]
    public sealed class MornCameraAdjusterMono : MonoBehaviour
    {
        [SerializeField] private Camera _targetCamera;
        [SerializeField] private MornCameraSettingSo _cameraSetting;
        private Vector2 _cachedRes;

        private void Start()
        {
            _cachedRes = new Vector2(Screen.width, Screen.height);
            AdjustCamera(_cachedRes);
        }

        private void Update()
        {
            var currentRes = new Vector2(Screen.width, Screen.height);
            if (_cachedRes != currentRes)
            {
                _cachedRes = currentRes;
                AdjustCamera(currentRes);
            }
        }

        private void AdjustCamera(Vector2 screenRes)
        {
            if (_cameraSetting == null)
            {
                return;
            }

            var currentAspect = screenRes.y / screenRes.x;
            var aimAspect = _cameraSetting.Resolution.y / _cameraSetting.Resolution.x;
            Rect newRect;
            if (currentAspect > aimAspect)
            {
                var gameRes = new Vector2(screenRes.x, screenRes.x * aimAspect);
                newRect = new Rect(0, (screenRes.y - gameRes.y) / screenRes.y / 2, 1, gameRes.y / screenRes.y);
            }
            else
            {
                newRect = new Rect(0, 0, 1, 1);
            }

            var curRect = _targetCamera.rect;
            if (curRect != newRect)
            {
                _targetCamera.rect = newRect;
            }
        }

        private void OnValidate()
        {
            if (_targetCamera == null)
            {
                _targetCamera = GetComponent<Camera>();
            }
        }
    }
}