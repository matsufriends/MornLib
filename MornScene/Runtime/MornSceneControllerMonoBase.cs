using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MornScene
{
    public abstract class MornSceneControllerMonoBase<TEnum> : MonoBehaviour where TEnum : struct, Enum
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] protected List<MornSceneMonoBase<TEnum>> _sceneList;
        [SerializeField] private TEnum _firstSceneType;
        private readonly Dictionary<TEnum, MornSceneMonoBase<TEnum>> _sceneDictionary = new();
        private readonly Stack<TEnum> _sceneUpdateStack = new();
        private readonly List<TEnum> _cachedUpdateList = new();

        private void Awake()
        {
            foreach (var scene in _sceneList)
            {
                _sceneDictionary.Add(scene.SceneType, scene);
                scene.Initialize();
                scene.OnChangeScene.Subscribe(LoadScene).AddTo(this);
                scene.OnAddScene.Subscribe(AddScene).AddTo(this);
                scene.OnRemoveScene.Subscribe(RemoveScene).AddTo(this);
            }
        }

        private void LoadScene(TEnum sceneType)
        {
            while (_sceneUpdateStack.TryPop(out var updateScene))
            {
                _sceneDictionary[updateScene].OnExitScene();
            }

            AddScene(sceneType);
        }

        private void AddScene(TEnum sceneType)
        {
            _sceneDictionary[sceneType].OnEnterScene();
            _sceneUpdateStack.Push(sceneType);
        }

        private void RemoveScene(TEnum sceneType)
        {
            _sceneDictionary[sceneType].OnExitScene();
            if (_sceneUpdateStack.TryPop(out var lastSceneType))
            {
                if (EqualityComparer<TEnum>.Default.Equals(sceneType, lastSceneType) == false)
                {
                    Debug.LogError($"{sceneType}=>最前のシーン{lastSceneType}から除去してください。");
                }
            }
            else
            {
                Debug.LogError($"{sceneType}=>AddSceneした記録が1つも存在しません。");
            }
        }

        public void ChangeSceneInEditor(string sceneName)
        {
            //ゲーム再生中に呼ばれていれば警告を出力する
            if (Application.isPlaying)
            {
                Debug.LogWarning($"再生中に関数[{nameof(ChangeSceneInEditor)}]を利用することはできません。");
                return;
            }

            foreach (var scene in _sceneList)
            {
                scene.SetSceneActive(scene.SceneType.ToString() == sceneName);
            }

            Debug.Log($"[{nameof(ChangeSceneInEditor)}]を実行しました。");
        }

        public void FindScenesInEditor()
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning($"再生中に関数[{nameof(FindScenesInEditor)}]を利用することはできません。");
                return;
            }

            _sceneList.Clear();
            _sceneList.AddRange(FindObjectsOfType<MornSceneMonoBase<TEnum>>());
            Debug.Log($"[{nameof(FindScenesInEditor)}]を実行しました。");
        }

        public void ApplyCanvasScaleInEditor()
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning($"再生中に関数[{nameof(ApplyCanvasScaleInEditor)}]を利用することはできません。");
                return;
            }

            foreach (var scene in _sceneList)
            {
                scene.ApplyCanvasScale(_width, _height);
            }

            Debug.Log($"[{nameof(ApplyCanvasScaleInEditor)}]を実行しました。");
        }

        public void LoadFirstScene()
        {
            LoadScene(_firstSceneType);
        }

        public void MyUpdate()
        {
            _cachedUpdateList.Clear();
            _cachedUpdateList.AddRange(_sceneUpdateStack);
            for (var i = 0; i < _cachedUpdateList.Count; i++)
            {
                _sceneDictionary[_cachedUpdateList[i]].OnUpdateScene(i == 0);
            }
        }
    }
}
