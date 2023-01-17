using System;
using MornLib.Mono;
using UniRx;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornLib.Scenes
{
    public abstract class MornSceneMonoBaseBase : MonoBehaviour
    {
        public abstract void SetSceneActive(bool isActive);
    }

    public abstract class MornSceneMonoBase<TEnum> : MornSceneMonoBaseBase where TEnum : Enum
    {
        [SerializeField] private MornCanvasSetterMono _canvasSetter;
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
            _canvasSetter?.SetActiveImmediate(true);
            _parent?.SetActive(true);
            OnEnterSceneImpl();
        }

        protected abstract void OnEnterSceneImpl();

        public void OnUpdateScene(bool isTopMost)
        {
            OnUpdateSceneImpl(isTopMost);
        }

        protected abstract void OnUpdateSceneImpl(bool isTopMost);

        public void OnExitScene()
        {
            ActiveSelf = false;
            _canvasSetter?.SetActiveImmediate(false);
            _parent?.SetActive(false);
            OnExitSceneImpl();
        }

        protected abstract void OnExitSceneImpl();

        public sealed override void SetSceneActive(bool isActive)
        {
            if (_canvasSetter != null)
            {
                _canvasSetter?.SetActiveImmediate(isActive);
            }

            if (_parent != null)
            {
                _parent.SetActive(isActive);
                _parent.name = $"Parent({isActive})";
            }

            gameObject.name = $"Scene{_sceneType.ToString()}({isActive})";
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MornSceneMonoBaseBase), true)]
    public class MornSceneMonoBaseBaseEditor : Editor
    {
        private MornSceneMonoBaseBase _target;

        public void OnEnable()
        {
            _target = (MornSceneMonoBaseBase)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("SetSceneActive"))
            {
                _target.SetSceneActive(true);
            }

            if (GUILayout.Button("SetSceneInactive"))
            {
                _target.SetSceneActive(false);
            }
        }
    }
#endif
}
