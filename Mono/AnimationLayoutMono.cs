using System;
using System.Collections.Generic;
using UnityEngine;
namespace MornLib.Mono {
    [RequireComponent(typeof(RectTransform))]
    public class AnimationLayoutMono : MonoBehaviour {
        [SerializeField] private Dir _dir;
        [SerializeField] private int _spacing;
        [SerializeField] private bool _ignoreTimeScale = true;
        [SerializeField] private List<RectTransform> _list = new();
        private const float _movement = 20;
        private void Update() {
            var offsetPos = Vector2.zero;
            var isUpDown = _dir == Dir.Up || _dir == Dir.Down;
            var dirVector = DirToVector(_dir);
            foreach(var rect in _list) {
                var curPos = rect.anchoredPosition;
                var dif = isUpDown ? offsetPos.y - curPos.y : offsetPos.x - curPos.x;
                var deltaTime = _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                var aimPos = curPos + (isUpDown ? Vector2.up : Vector2.right) * (dif * Mathf.Min(1,deltaTime * _movement));
                rect.anchoredPosition =  aimPos;
                offsetPos             += dirVector * ((isUpDown ? rect.rect.size.y : rect.rect.size.x) + _spacing);
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
            _list.Clear();
            for(var i = transform.childCount - 1;i >= 0;i--) {
                if(transform.GetChild(i).TryGetComponent<RectTransform>(out var rect)) _list.Add(rect);
            }
        }
        private enum Dir {
            Up
           ,Left
           ,Right
           ,Down
        }
    }
}
