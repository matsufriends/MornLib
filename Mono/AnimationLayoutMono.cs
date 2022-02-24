using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace MornLib.Mono {
    [RequireComponent(typeof(RectTransform))]
    public class AnimationLayoutMono : MonoBehaviour {
        [SerializeField] private Dir                        _dir;
        [SerializeField] private RectTransform              _rect;
        private                  IEnumerable<RectTransform> _list;
        private const            float                      _movement = 10;
        private void Update() {
            if(_list == null) return;
            var offsetPos = Vector2.zero;
            var isUpDown  = _dir == Dir.Up || _dir == Dir.Down;
            var dirVector = DirToVector(_dir);
            foreach(var rect in _list.Reverse()) {
                var curPos = rect.anchoredPosition;
                var dif    = isUpDown ? offsetPos.y - curPos.y : offsetPos.x - curPos.x;
                var aimPos = curPos + (isUpDown ? Vector2.up : Vector2.right) * (dif * (Time.deltaTime * _movement));
                rect.anchoredPosition =  aimPos;
                offsetPos             += dirVector * ((isUpDown ? rect.rect.max.y : rect.rect.max.x) * 2);
            }
        }
        private static Vector2 DirToVector(Dir dir) {
            switch(dir) {
                case Dir.Up:    return Vector2.up;
                case Dir.Left:  return Vector2.left;
                case Dir.Right: return Vector2.right;
                case Dir.Down:  return Vector2.down;
                default:        throw new ArgumentOutOfRangeException();
            }
        }
        private void OnTransformChildrenChanged() {
            _list = GetComponentsInChildren<RectTransform>().Where(x => x != _rect);
        }
        private enum Dir {
            Up
           ,Left
           ,Right
           ,Down
        }
    }
}