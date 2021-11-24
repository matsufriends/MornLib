using UnityEngine;
namespace MornLib.Mono {
    [RequireComponent(typeof(RectTransform))]
    public class TcgRectMono : MonoBehaviour {
        //Field
        [SerializeField] private RectTransform _ownRect;
        private                  int           _index;
        //Property
        public int     Index => _index;
        public Vector2 Size  => _ownRect.sizeDelta;
        public Vector2 Scale => _ownRect.localScale;
        //Method
        public void SetIndex(int index) {
            _index = index;
        }
        public void RemoveIndex(int index) {
            if(index < _index) _index--;
        }
        public void SetUpdate(Vector3 pos,Quaternion rotation,Vector3 scale,float transition) {
            _ownRect.localScale       = Vector3.Lerp(_ownRect.localScale,scale,transition);
            _ownRect.localRotation    = Quaternion.Lerp(_ownRect.localRotation,rotation,transition);
            _ownRect.anchoredPosition = Vector3.Lerp(_ownRect.anchoredPosition,pos,transition);
        }
    }
}