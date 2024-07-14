using System;
using UnityEngine;

namespace MornLib.PopUps
{
    public interface IMornPopUpManager
    {
        void Init(RectTransform rect, Action<IMornPopUpCaller> setText);
        void Show(IMornPopUpCaller mornPopUpCaller);
        void Hide();
    }
}