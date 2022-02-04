using System;
using MornLib.Singletons;
using UnityEngine;
namespace MornLib.Mono {
    public interface IPopUpManager : ISingleton {
        void Init(RectTransform rect,Action<string,string> setText);
        void Show(IPopUpCaller  popUpCaller);
        void Hide();
    }
}