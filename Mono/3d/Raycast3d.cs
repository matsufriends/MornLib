using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace MornLib.Mono._3d {
    public class Raycast3d : MonoBehaviour,IPointerEvent {
        [SerializeField] private bool _isOnMouseRight;
        [SerializeField] private bool _isOnMouseMiddle;
        [SerializeField] private bool _isOnMouseLeft;
        private readonly Subject<Unit> _mouseUpSubject = new Subject<Unit>();
        private readonly Subject<Unit> _mouseDownSubject = new Subject<Unit>();
        private readonly Subject<Unit> _mouseClickSubject = new Subject<Unit>();
        private bool _isOver;
        private bool _isDrag;
        IObservable<Unit> IPointerEvent.OnPointerEnter => gameObject.OnMouseEnterAsObservable();
        IObservable<Unit> IPointerEvent.OnPointerExit => gameObject.OnMouseExitAsObservable();
        IObservable<Unit> IPointerEvent.OnPointerUp => _mouseUpSubject;
        IObservable<Unit> IPointerEvent.OnPointerDown => _mouseDownSubject;
        IObservable<Unit> IPointerEvent.OnPointerClick => _mouseClickSubject;
        private void Awake() {
            ((IPointerEvent) this).OnPointerEnter.Subscribe(_ => _isOver = true).AddTo(this);
            ((IPointerEvent) this).OnPointerExit.Subscribe(_ => _isOver  = false).AddTo(this);
        }
        private void Update() {
            if(_isDrag) UpdateDrag();
            if(_isOver) UpdateOver();
        }
        private void UpdateDrag() {
            var up = false;
            up |= _isOnMouseLeft && Input.GetMouseButtonUp(0);
            up |= _isOnMouseRight && Input.GetMouseButtonUp(1);
            up |= _isOnMouseMiddle && Input.GetMouseButtonUp(2);
            if(up) {
                _mouseUpSubject.OnNext(Unit.Default);
                if(_isOver) _mouseClickSubject.OnNext(Unit.Default);
                _isDrag = false;
            }
        }
        private void UpdateOver() {
            var down = false;
            down |= _isOnMouseLeft && Input.GetMouseButtonDown(0);
            down |= _isOnMouseRight && Input.GetMouseButtonDown(1);
            down |= _isOnMouseMiddle && Input.GetMouseButtonDown(2);
            if(down) {
                _mouseDownSubject.OnNext(Unit.Default);
                _isDrag = true;
            }
        }
    }
}