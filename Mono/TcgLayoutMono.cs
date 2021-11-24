using System;
using System.Collections.Generic;
using MornLib.Extensions;
using UnityEngine;
namespace MornLib.Mono {
    public class TcgLayoutMono : MonoBehaviour {
        [SerializeField] private RectTransform     _ownRect;
        [SerializeField] private int               _spacing;
        [SerializeField] private int               _radius;
        [SerializeField] private int               _angle;
        [SerializeField] private Vector3           _focusScale;
        [SerializeField] private List<TcgRectMono> _rectList;
        [SerializeField] private float             _transitionK;
        private                  int               _index;
        //Property
        public int FocusIndex => _index;
        //Method
        [ContextMenu("DebugUpdate")]
        private void Update() {
            var transition = _transitionK * Time.deltaTime;
            var rectCount  = _rectList.Count;
            if(rectCount == 0) return;
            var isLeft        = true;
            var leftWidthSum  = 0f;
            var focusWidth    = 0f;
            var rightWidthSum = 0f;
            for(var i = 0;i < rectCount;i++) {
                var rect = _rectList[i];
                if(i == _index) {
                    focusWidth = rect.Size.x * rect.Scale.x;
                    isLeft     = false;
                    continue;
                }
                if(isLeft) {
                    leftWidthSum += rect.Size.x * rect.Scale.x + _spacing;
                } else {
                    rightWidthSum += rect.Size.x * rect.Scale.x + _spacing;
                }
            }
            var widthSum  = leftWidthSum + focusWidth + rightWidthSum;
            var rectWidth = _ownRect.sizeDelta.x;
            var shrinkK   = 1f;
            if(widthSum > rectWidth) {
                var over = widthSum - rectWidth;
                shrinkK  = 1 - over / (leftWidthSum + rightWidthSum);
                widthSum = rectWidth;
            }
            var leftX = -widthSum / 2f;
            for(var i = 0;i < rectCount;i++) {
                var rect      = _rectList[i];
                var aimScale  = i == _index ? _focusScale : Vector3.one;
                var width     = (rect.Size.x * rect.Scale.x + _spacing) * (i == _index ? 1 : shrinkK);
                var ratio     = (leftX + width / 2f) / (widthSum / 2f); //-1 ~ 1
                var aimAngle  = i == _index ? 0 : -_angle * ratio;
                var rad       = _angle * ratio * Mathf.Deg2Rad;
                var rotatePos = new Vector2(Mathf.Sin(rad),Mathf.Cos(rad) - 1) * _radius;
                rotatePos.x = leftX + width / 2f;
                var constantPos = new Vector2(rotatePos.x,-_ownRect.sizeDelta.y / 2f + rect.Size.y * aimScale.y / 2f);
                var aimPos      = i == _index ? constantPos : rotatePos;
                rect.SetUpdate(aimPos,Quaternion.Euler(0,0,aimAngle),aimScale,transition);
                leftX += width;
            }
        }
        public void SetItem(TcgRectMono tcgRect) {
            _rectList.Add(tcgRect);
            tcgRect.SetIndex(_rectList.Count - 1);
        }
        public void DestroyItem(TcgRectMono tcgRect) {
            if(tcgRect.Index < 0 || _rectList.Count <= tcgRect.Index) throw new Exception("不正なIndexです");
            _rectList.RemoveAt(tcgRect.Index);
            foreach(var rect in _rectList) rect.RemoveIndex(tcgRect.Index);
            Destroy(tcgRect.gameObject);
        }
        public void DestroyAll() {
            _rectList.Clear();
            transform.DestroyChildren();
            _index = -1;
        }
        public void SetFocus(int index) {
            _index = index;
        }
    }
}