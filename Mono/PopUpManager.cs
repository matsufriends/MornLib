using System;
using MornLib.Cores;
using MornLib.Singletons;
using UnityEngine;
namespace MornLib.Mono {
    public class PopUpManager : Singleton<PopUpManager,IPopUpManager>,IPopUpManager {
        private       bool                  _initialized;
        private       RectTransform         _popRect;
        private       Action<string,string> _setText;
        private const int                   _width     = 1920;
        private const int                   _height    = 1080;
        private const int                   _space     = 50;
        private const int                   _deadSpace = 50;
        void IPopUpManager.Init(RectTransform rect,Action<string,string> setText) {
            _initialized = true;
            _popRect     = rect;
            _setText     = setText;
        }
        void IPopUpManager.Show(IPopUpCaller popUpCaller) {
            if(_initialized == false) {
                MornLog.Warning("PopUpはまだセットアップされていません");
                return;
            }
            _popRect.gameObject.SetActive(true);
            _popRect.position = (Vector2) Input.mousePosition;
            var ancPos = _popRect.anchoredPosition;
            if(ancPos.x < _width - _popRect.sizeDelta.x - _space - _deadSpace) {
                ancPos.x += _space + _popRect.sizeDelta.x / 2f;
            } else {
                ancPos.x -= _space + _popRect.sizeDelta.x / 2f;
            }
            if(ancPos.y < _height - _popRect.sizeDelta.y - _space - _deadSpace) {
                ancPos.y += _space + _popRect.sizeDelta.y / 2f;
            } else {
                ancPos.y -= _space + _popRect.sizeDelta.y / 2f;
            }
            _popRect.anchoredPosition = ancPos;
            _setText(popUpCaller.TitleText,popUpCaller.DetailText);
        }
        void IPopUpManager.Hide() {
            if(_initialized == false) {
                MornLog.Warning("PopUpはまだセットアップされていません");
                return;
            }
            _popRect.gameObject.SetActive(false);
        }
        public void Instanced() { }
    }
}