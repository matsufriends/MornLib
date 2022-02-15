using System;
using MornLib.Cores;
using MornLib.Singletons;
using UnityEngine;
namespace MornLib.Mono {
    public class PopUpManager : Singleton<PopUpManager,IPopUpManager>,IPopUpManager {
        private       bool                  _initialized;
        private       RectTransform         _popRect;
        private       Action<string,string> _setText;
        private const int                   _width           = 1920;
        private const int                   _height          = 1080;
        private const int                   _space           = 50;
        private const int                   _screenDeadSpace = 50;
        private const int                   _baseY           = 110;
        private const int                   _offset          = 40;
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
            _popRect.position = popUpCaller.CenterPos;
            var ancPos     = _popRect.anchoredPosition;
            var detailText = popUpCaller.DetailText;
            _popRect.sizeDelta = new Vector2(detailText.LongestLengthBySplit('\n') * _offset,_baseY + _offset * detailText.MatchCount('\n'));
            if(ancPos.x < _width - _popRect.sizeDelta.x - _space - _screenDeadSpace) {
                ancPos.x += _space + _popRect.sizeDelta.x / 2f;
            } else {
                ancPos.x -= _space + _popRect.sizeDelta.x / 2f;
            }
            if(ancPos.y < _height - _popRect.sizeDelta.y - _space - _screenDeadSpace) {
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