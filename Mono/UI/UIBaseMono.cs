using System;
using MornLib.Extensions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
namespace MornLib.Mono.UI {
    public class UIBaseMono : MonoBehaviour {
        [SerializeField] private UIBehaviour _ui;
        [SerializeField] private bool _isOnMouseRight;
        [SerializeField] private bool _isOnMouseMiddle;
        [SerializeField] private bool _isOnMouseLeft;
        public IObservable<Unit> OnPointerEnter => _ui.OnPointerEnterAsObservable().Select(_ => Unit.Default);
        public IObservable<Unit> OnPointerExit => _ui.OnPointerExitAsObservable().Select(_ => Unit.Default);
        public IObservable<Unit> OnPointerUp => _ui.OnPointerUpAsObservable().Where(Match).Select(_ => Unit.Default);
        public IObservable<Unit> OnPointerDown => _ui.OnPointerDownAsObservable().Where(Match).Select(_ => Unit.Default);
        public IObservable<Unit> OnPointerClick => _ui.OnPointerClickAsObservable().Where(Match).Select(_ => Unit.Default);
        private void Awake() {
            var user = GetComponent<IUIBaseUser>();
            if(user == null) return;
            OnPointerEnter.Subscribe(_ => user.OnSelect()).AddTo(this);
            OnPointerExit.Subscribe(_ => user.OnDeSelect()).AddTo(this);
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