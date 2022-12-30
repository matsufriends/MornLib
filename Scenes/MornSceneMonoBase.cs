using System;
using MornLib.Mono;
using UniRx;
using UnityEngine;

namespace MornLib.Scenes
{
    public abstract class MornSceneMonoBase<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] private MornCanvasGroupSetterMono _canvasGroupSetter;
        [SerializeField] private GameObject _parent;
        [SerializeField] private TEnum _sceneType;
        private readonly Subject<TEnum> _loadSceneSubject = new();
        private readonly Subject<TEnum> _addSceneSubject = new();
        private readonly Subject<TEnum> _removeSceneSubject = new();
        public TEnum SceneType => _sceneType;
        public IObservable<TEnum> OnLoadScene => _loadSceneSubject;
        public IObservable<TEnum> OnAddScene => _addSceneSubject;
        public IObservable<TEnum> OnRemoveScene => _removeSceneSubject;
        protected bool ActiveSelf { get; private set; }
        public abstract void MyAwake();

        protected void LoadScene(TEnum sceneType)
        {
            _loadSceneSubject.OnNext(sceneType);
        }

        protected void AddScene(TEnum sceneType)
        {
            _addSceneSubject.OnNext(sceneType);
        }

        protected void RemoveScene(TEnum sceneType)
        {
            _removeSceneSubject.OnNext(sceneType);
        }

        public void OnEnterScene()
        {
            ActiveSelf = true;
            _canvasGroupSetter?.SetActiveImmediate(true);
            _parent?.SetActive(true);
            OnEnterSceneImpl();
        }

        protected abstract void OnEnterSceneImpl();

        public void OnUpdateScene()
        {
            OnUpdateSceneImpl();
        }

        protected abstract void OnUpdateSceneImpl();

        public void OnExitScene()
        {
            ActiveSelf = false;
            _canvasGroupSetter?.SetActiveImmediate(false);
            _parent?.SetActive(false);
            OnExitSceneImpl();
        }

        protected abstract void OnExitSceneImpl();
    }
}
