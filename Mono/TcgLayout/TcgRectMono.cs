using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace MornLib.Mono.TcgLayout {
    [RequireComponent(typeof(RectTransform))]
    public sealed class TcgRectMono : MonoBehaviour,ITcgRectController,IPointerEnterHandler,IPointerExitHandler {
        [SerializeField] private RectTransform _ownRect;
        private ITcgRectUser _tcgRectUser;
        private Action _selected;
        private Action _deselected;
        private int _index;
        int ITcgRectController.Index => _index;
        Vector2 ITcgRectController.Size => _ownRect.sizeDelta;
        Vector2 ITcgRectController.Scale => _ownRect.localScale;
        private void Awake() => _tcgRectUser = GetComponent<ITcgRectUser>();
        private void Reset() => _ownRect = GetComponent<RectTransform>();
        void ITcgRectController.Init(Action selected,Action deselected) {
            _selected   = selected;
            _deselected = deselected;
        }
        void ITcgRectController.SetIndex(int index) => _index = index;
        void ITcgRectController.RemoveIndex(int index) {
            if(index < _index) _index--;
        }
        void ITcgRectController.SetUpdate(Vector3 pos,Quaternion rotation,Vector3 scale,float transition) {
            _ownRect.localScale       = Vector3.Lerp(_ownRect.localScale,scale,transition);
            _ownRect.localRotation    = Quaternion.Lerp(_ownRect.localRotation,rotation,transition);
            _ownRect.anchoredPosition = Vector3.Lerp(_ownRect.anchoredPosition,pos,transition);
        }
        void ITcgRectController.ExeDestroy() => Destroy(gameObject);
        void ITcgRectController.Clicked() => _tcgRectUser?.OnClick();
        void ITcgRectController.Select() => _tcgRectUser?.OnSelect();
        void ITcgRectController.Deselect() => _tcgRectUser?.OnDeselect();
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => _selected?.Invoke();
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => _deselected?.Invoke();
    }
}
