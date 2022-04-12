using System;
using MornLib.Extensions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
namespace MornLib.Mono.UI {
    public class UIBaseMono : MonoBehaviour,IPointerEvent {
        [SerializeField] private UIBehaviour _ui;
        [SerializeField] private bool _isOnMouseRight;
        [SerializeField] private bool _isOnMouseMiddle;
        [SerializeField] private bool _isOnMouseLeft = true;
        [SerializeField] private bool _isReactChild;
        private bool _isOver;
        private readonly Subject<Unit> _pointerExitSubject = new Subject<Unit>();
        public IObservable<Unit> OnPointerEnter
            => _ui.OnPointerEnterAsObservable().Where(x => _isReactChild || x.pointerEnter == gameObject).Select(_ => Unit.Default);
        public IObservable<Unit> OnPointerExit => _pointerExitSubject;
        public IObservable<Unit> OnPointerUp => _ui.OnPointerUpAsObservable().Where(Match).Select(_ => Unit.Default);
        public IObservable<Unit> OnPointerDown => _ui.OnPointerDownAsObservable().Where(Match).Select(_ => Unit.Default);
        public IObservable<Unit> OnPointerClick => _ui.OnPointerClickAsObservable().Where(Match).Select(_ => Unit.Default);
        private void Awake() {
            OnPointerEnter.Subscribe(_ => _isOver = true).AddTo(this);
            _ui.OnPointerExitAsObservable()
               .Subscribe(
                    _ => {
                        _isOver = false;
                        _pointerExitSubject.OnNext(Unit.Default);
                    }
                )
               .AddTo(this);
            gameObject.OnDisableAsObservable()
               .Where(_ => _isOver)
               .Subscribe(
                    _ => {
                        _isOver = false;
                        _pointerExitSubject.OnNext(Unit.Default);
                    }
                )
               .AddTo(this);
            var users = GetComponents<IUIBaseUser>();
            if(users == null) return;
            ((IPointerEvent) this).OnPointerEnter.Subscribe(
                    _ => {
                        foreach(var user in users) user.OnSelect();
                    }
                )
               .AddTo(this);
            ((IPointerEvent) this).OnPointerExit.Subscribe(
                    _ => {
                        foreach(var user in users) user.OnDeSelect();
                    }
                )
               .AddTo(this);
            ((IPointerEvent) this).OnPointerClick.Subscribe(
                    _ => {
                        foreach(var user in users) user.OnClick();
                    }
                )
               .AddTo(this);
        }
        private bool Match(PointerEventData eventData) {
            if(_isOnMouseRight && eventData.IsRightClick()) return true;
            if(_isOnMouseMiddle && eventData.IsMiddleClick()) return true;
            if(_isOnMouseLeft && eventData.IsLeftClick()) return true;
            return false;
        }
        private void Reset() {
            _ui = GetComponent<UIBehaviour>();
        }
    }
}