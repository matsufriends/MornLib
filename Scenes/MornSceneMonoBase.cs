using System;
using UniRx;
using UnityEngine;

namespace MornLib.Scenes
{
    public abstract class MornSceneMonoBase<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] private TEnum _sceneType;
        private readonly Subject<TEnum> _changeSceneSubject = new();
        public TEnum SceneType => _sceneType;
        public IObservable<TEnum> OnChangeScene => _changeSceneSubject;

        protected void ChangeScene(TEnum sceneType)
        {
            _changeSceneSubject.OnNext(sceneType);
        }

        public abstract void OnEnterScene();
        public abstract void SceneUpdate();
        public abstract void OnExitScene();
    }
}
