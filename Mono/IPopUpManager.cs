using System;
using UnityEngine;
namespace MornLib.Mono {
    public interface IPopUpManager {
        void Init(RectTransform rect,Action<IPopUpCaller> setText);
        void Show(IPopUpCaller  popUpCaller);
        void Hide();
    }
}