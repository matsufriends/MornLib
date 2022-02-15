using UnityEngine;
namespace MornLib.Mono {
    public interface IPopUpCaller {
        Vector2 CenterPos  { get; }
        string  TitleText  { get; }
        string  DetailText { get; }
    }
}