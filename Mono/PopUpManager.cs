using System;
using MornLib.Cores;
using MornLib.Singletons;
using UnityEngine;
namespace MornLib.Mono {
    public class PopUpManager : Singleton<PopUpManager,IPopUpManager>,IPopUpManager,ISingleton {
        private       bool                  _initialized;
        private       RectTransform         _popRect;
        private       Action<IPopUpCaller> _setText;
        private       float                 _cachedLastShowTime;
        void IPopUpManager.Init(RectTransform rect,Action<IPopUpCaller> setText) {
            _initialized = true;
            _popRect     = rect;
            _setText     = setText;
        }
        void IPopUpManager.Show(IPopUpCaller popUpCaller) {
            if(_initialized == false) {
                MornLog.Warning("PopUpはまだセットアップされていません");
                return;
            }
            _cachedLastShowTime = Time.unscaledTime;
            _popRect.gameObject.SetActive(true);
            _setText(popUpCaller);
        }
        void IPopUpManager.Hide() {
            if(_initialized == false) {
                MornLog.Warning("PopUpはまだセットアップされていません");
                return;
            }
            if(Time.unscaledTime - _cachedLastShowTime == 0) return;
            _popRect.gameObject.SetActive(false);
        }
        public void Instanced() { }
    }
}