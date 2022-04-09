using System;
using UnityEngine;
namespace MornLib.Mono.TcgLayout {
    public interface ITcgRectController {
        public int Index { get; }
        public Vector2 Size { get; }
        public Vector2 Scale { get; }
        public void Init(Action       selected,Action deselected);
        public void SetIndex(int      index);
        public void RemoveIndex(int   index);
        public void SetUpdate(Vector3 pos,Quaternion rotation,Vector3 scale,float transition);
        public void ExeDestroy();
        public void Clicked();
        public void Select();
        public void Deselect();
    }
}