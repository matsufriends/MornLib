using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornCamera
{
    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public sealed class MornCameraAdjusterMono : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private MornCameraSettingSo cameraSetting;
        private Vector2 cachedRes;

        private void Start()
        {
            cachedRes = new Vector2(Screen.width, Screen.height);
            AdjustCamera(cachedRes);
        }

        private void Update()
        {
            var currentRes = new Vector2(Screen.width, Screen.height);
            if (cachedRes != currentRes)
            {
                cachedRes = currentRes;
                AdjustCamera(currentRes);
            }
        }

        private void AdjustCamera(Vector2 screenRes)
        {
            var currentAspect = screenRes.y / screenRes.x;
            var aimAspect = cameraSetting.Resolution.y / cameraSetting.Resolution.x;
            Rect newRect;
            if (currentAspect > aimAspect)
            {
                var gameRes = new Vector2(screenRes.x, screenRes.x * aimAspect);
                newRect= new Rect(0, (screenRes.y - gameRes.y) / screenRes.y / 2, 1, gameRes.y / screenRes.y);
            }
            else
            {
                newRect = new Rect(0, 0, 1, 1);
            }

            var curRect = targetCamera.rect;
            if (curRect != newRect)
            {
                targetCamera.rect = newRect;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(targetCamera);
                }
#endif
            }
        }

        private void OnValidate()
        {
            if (targetCamera == null)
            {
                targetCamera = GetComponent<Camera>();
            }
        }
    }
}