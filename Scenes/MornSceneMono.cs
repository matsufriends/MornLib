using System;
using UnityEngine;
namespace MornLib.Scenes {
    public abstract class MornSceneMono : MonoBehaviour {
        protected void HideOwn() => Destroy(gameObject);
        protected void HideOwnAndShow(Type sceneType) {
            HideOwn();
            Show(sceneType);
        }
        protected void Show(Type sceneType) => MornSceneManagerMono.Instance.GenerateScene(sceneType);
    }
}
