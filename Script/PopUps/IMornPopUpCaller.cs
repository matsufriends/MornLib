using UnityEngine;

namespace MornLib.PopUps
{
    public interface IMornPopUpCaller
    {
        Vector2 CenterPos { get; }
        string TitleText { get; }
        string DetailText { get; }
    }
}
