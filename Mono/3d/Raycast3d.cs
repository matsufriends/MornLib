using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MornLib.Mono._3d
{
    public class Raycast3d : MonoBehaviour, IPointerEvent
    {
        [SerializeField] private bool _isOnMouseRight;
        [SerializeField] private bool _isOnMouseMiddle;
        [SerializeField] private bool _isOnMouseLeft;
        private readonly Subject<MouseClickSet> _mouseUpSubject = new();
        private readonly Subject<MouseClickSet> _mouseDownSubject = new();
        private readonly Subject<MouseClickSet> _mouseClickSubject = new();
        private bool _isOver;
        private bool _isDrag;
        public IObservable<Unit> OnPointerEnter => gameObject.OnMouseEnterAsObservable();
        public IObservable<Unit> OnPointerExit => gameObject.OnMouseExitAsObservable();
        public IObservable<MouseClickSet> OnPointerUp => _mouseUpSubject;
        public IObservable<MouseClickSet> OnPointerDown => _mouseDownSubject;
        public IObservable<MouseClickSet> OnPointerClick => _mouseClickSubject;

        private void Awake()
        {
            OnPointerEnter.Subscribe(_ => _isOver = true).AddTo(this);
            OnPointerExit.Subscribe(_ => _isOver = false).AddTo(this);
        }

        private void Update()
        {
            if (_isDrag)
            {
                UpdateDrag();
            }

            if (_isOver)
            {
                UpdateOver();
            }
        }

        private void UpdateDrag()
        {
            var rightUp = _isOnMouseRight && Input.GetMouseButtonUp(1);
            var middleUp = _isOnMouseMiddle && Input.GetMouseButtonUp(2);
            var leftUp = _isOnMouseLeft && Input.GetMouseButtonUp(0);
            if (rightUp || middleUp || leftUp)
            {
                _mouseUpSubject.OnNext(new MouseClickSet(rightUp, middleUp, leftUp));
                if (_isOver)
                {
                    _mouseClickSubject.OnNext(new MouseClickSet(rightUp, middleUp, leftUp));
                }

                _isDrag = false;
            }
        }

        private void UpdateOver()
        {
            var rightDown = _isOnMouseRight && Input.GetMouseButtonDown(1);
            var middleDown = _isOnMouseMiddle && Input.GetMouseButtonDown(2);
            var leftDown = _isOnMouseLeft && Input.GetMouseButtonDown(0);
            if (rightDown || middleDown || leftDown)
            {
                _mouseDownSubject.OnNext(new MouseClickSet(rightDown, middleDown, leftDown));
                _isDrag = true;
            }
        }
    }
}