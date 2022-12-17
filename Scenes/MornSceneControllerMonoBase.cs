using System;
using System.Collections.Generic;
using MornLib.Cores;
using UniRx;
using UnityEngine;

namespace MornLib.Scenes
{
    public abstract class MornSceneControllerMonoBase<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] protected List<MornSceneMonoBase<TEnum>> _sceneList;
        [SerializeField] private TEnum _firstSceneType;
        private readonly Dictionary<TEnum, MornSceneMonoBase<TEnum>> _sceneDictionary = new();
        private readonly Stack<TEnum> _sceneUpdateStack = new();

        private void Awake()
        {
            foreach (var scene in _sceneList)
            {
                _sceneDictionary.Add(scene.SceneType, scene);
                scene.MyAwake();
                scene.OnLoadScene.Subscribe(LoadScene).AddTo(this);
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
                    MornLog.Log($"{sceneType}=>最新のシーン{lastSceneType}から除去してください。");
                }
            }
            else
            {
                MornLog.Log($"{sceneType}=>AddSceneした記録が1つも存在しません。");
            }
        }

        public void LoadFirstScene()
        {
            LoadScene(_firstSceneType);
        }

        public void MyUpdate()
        {
            foreach (var sceneType in _sceneUpdateStack)
            {
                _sceneDictionary[sceneType].OnUpdateScene();
            }
        }
    }
}
