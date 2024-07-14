using System;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;

[assembly: InternalsVisibleTo("MornScene.Editor")]

namespace MornScene
{
    public abstract class MornSceneMonoBase : MonoBehaviour
    {
        [SerializeField] private MornSceneCanvasMono _sceneCanvas;
        [SerializeField] private GameObject _root;
        private readonly Subject<MornSceneDataSo> _onAddScene = new();
        private readonly Subject<MornSceneDataSo> _onChangeScene = new();
        private readonly Subject<Unit> _onEnterSceneSubject = new();
        private readonly Subject<Unit> _onExitSceneSubject = new();
        private readonly Subject<MornSceneDataSo> _onRemoveScene = new();
        private readonly Subject<Unit> _onUpdateSceneSubject = new();
        protected bool ActiveSelf { get; private set; }
        public IObservable<Unit> OnEnterSceneRx => _onEnterSceneSubject;
        public IObservable<Unit> OnUpdateSceneRx => _onUpdateSceneSubject;
        public IObservable<Unit> OnExitSceneRx => _onExitSceneSubject;
        public IObservable<MornSceneDataSo> OnChangeSceneRx => _onChangeScene;
        public IObservable<MornSceneDataSo> OnAddSceneRx => _onAddScene;
        public IObservable<MornSceneDataSo> OnRemoveSceneRx => _onRemoveScene;

        protected void ChangeScene(MornSceneDataSo sceneData)
        {
            _onChangeScene.OnNext(sceneData);
        }

        protected void AddScene(MornSceneDataSo sceneData)
        {
            _onAddScene.OnNext(sceneData);
        }

        protected void RemoveScene(MornSceneDataSo sceneData)
        {
            _onRemoveScene.OnNext(sceneData);
        }

        internal void Initialize(MornSceneDataSo sceneData)
        {
            OnInitializeImpl(sceneData);
        }

        internal void OnEnterScene(MornSceneDataSo sceneData)
        {
            ActiveSelf = true;
            SetSceneActive(sceneData, true);
            OnEnterSceneImpl(sceneData);
            _onEnterSceneSubject.OnNext(Unit.Default);
        }

        internal void OnUpdateScene(MornSceneDataSo sceneData, bool isTop)
        {
            OnUpdateSceneImpl(sceneData, isTop);
            _onUpdateSceneSubject.OnNext(Unit.Default);
        }

        internal void OnExitScene(MornSceneDataSo sceneData)
        {
            ActiveSelf = false;
            SetSceneActive(sceneData, false);
            OnExitSceneImpl(sceneData);
            _onExitSceneSubject.OnNext(Unit.Default);
        }

        protected abstract void OnInitializeImpl(MornSceneDataSo sceneData);
        protected abstract void OnEnterSceneImpl(MornSceneDataSo sceneData);
        protected abstract void OnUpdateSceneImpl(MornSceneDataSo sceneData, bool isTop);
        protected abstract void OnExitSceneImpl(MornSceneDataSo sceneData);

        internal void SetSceneActive(MornSceneDataSo sceneData, bool isActive)
        {
            var sceneName = sceneData.SceneName;
            if (_sceneCanvas != null)
            {
                _sceneCanvas.SetActiveImmediate(isActive);
                _sceneCanvas.name = $"[{sceneName}] Canvas";
            }

            if (_root != null)
            {
                _root.SetActive(isActive);
                _root.name = $"[{sceneName}] Root";
            }

            gameObject.name = $"[{sceneName}] {(isActive ? "Active" : "")}";
        }
    }
}