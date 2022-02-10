using UnityEngine;
using UnityEngine.UI;
namespace MornLib.Mono {
    [RequireComponent(typeof(Image))]
    public class ImageGizmoMono : MonoBehaviour {
        [SerializeField] private Image _image;
        private void Reset() {
            _image = GetComponent<Image>();
        }
        #if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow * _image.canvasRenderer.GetInheritedAlpha();
            Gizmos.DrawWireCube(GetCenterPosition(_image.rectTransform),_image.rectTransform.rect.size * _image.rectTransform.lossyScale);
        }
        private static Vector2 GetCenterPosition(RectTransform rect) {
            var position = rect.transform.position;
            if(rect.pivot != new Vector2(0.5f,0.5f)) {
                var scaleX = rect.transform.lossyScale.x;
                var scaleY = rect.transform.lossyScale.y;
                var x      = rect.rect.width / 2f * scaleX;
                var y      = rect.rect.height / 2f * scaleY;
                position.x += Mathf.Lerp(x,-x,rect.pivot.x);
                position.y += Mathf.Lerp(y,-y,rect.pivot.y);
            }
            return position;
        }
        #endif
    }
}