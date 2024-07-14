using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib.Mono.TcgLayout
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class MornTcgRectMono : MonoBehaviour, IMornTcgRectController, IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] private RectTransform _ownRect;
        private Action _deselected;
        private int _index;
        private IMornTcgRectUser _mornTcgRectUser;
        private Action _selected;

        private void Awake()
        {
            _mornTcgRectUser = GetComponent<IMornTcgRectUser>();
        }

        private void Reset()
        {
            _ownRect = GetComponent<RectTransform>();
        }

        int IMornTcgRectController.Index => _index;
        Vector2 IMornTcgRectController.Size => _ownRect.sizeDelta;
        Vector2 IMornTcgRectController.Scale => _ownRect.localScale;

        void IMornTcgRectController.Init(Action selected, Action deselected)
        {
            _selected = selected;
            _deselected = deselected;
        }

        void IMornTcgRectController.SetIndex(int index)
        {
            _index = index;
        }

        void IMornTcgRectController.RemoveIndex(int index)
        {
            if (index < _index) _index--;
        }

        void IMornTcgRectController.SetUpdate(Vector3 pos, Quaternion rotation, Vector3 scale, float transition)
        {
            _ownRect.localScale = Vector3.Lerp(_ownRect.localScale, scale, transition);
            _ownRect.localRotation = Quaternion.Lerp(_ownRect.localRotation, rotation, transition);
            _ownRect.anchoredPosition = Vector3.Lerp(_ownRect.anchoredPosition, pos, transition);
        }

        void IMornTcgRectController.ExeDestroy()
        {
            Destroy(gameObject);
        }

        void IMornTcgRectController.Clicked()
        {
            _mornTcgRectUser?.OnClick();
        }

        void IMornTcgRectController.Select()
        {
            _mornTcgRectUser?.OnSelect();
        }

        void IMornTcgRectController.Deselect()
        {
            _mornTcgRectUser?.OnDeselect();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _selected?.Invoke();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _deselected?.Invoke();
        }
    }
}