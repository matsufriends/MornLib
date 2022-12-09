using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MornLib.Scenes
{
    public abstract class MornSceneControllerMonoBase<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] private List<MornSceneMonoBase<TEnum>> _sceneList;
        [SerializeField] private TEnum _sceneType;
        private readonly Dictionary<TEnum, MornSceneMonoBase<TEnum>> _sceneDictionary = new();

        private void Awake()
        {
            foreach (var scene in _sceneList)
            {
                _sceneDictionary.Add(scene.SceneType, scene);
                scene.OnChangeScene.Subscribe(ChangeScene).AddTo(this);
                scene.OnAddScene.Subscribe(AddScene).AddTo(this);
                scene.OnRemoveScene.Subscribe(RemoveScene).AddTo(this);
            }
        }

        private void Start()
        {
            ChangeScene(_sceneType);
        }

        private void ChangeScene(TEnum sceneType)
        {
            _sceneDictionary[_sceneType].OnExitScene();
            _sceneType = sceneType;
            _sceneDictionary[_sceneType].OnEnterScene();
        }

        private void AddScene(TEnum sceneType)
        {
            _sceneDictionary[_sceneType].OnEnterScene();
        }

        private void RemoveScene(TEnum sceneType)
        {
            _sceneDictionary[_sceneType].OnExitScene();
        }

        public void MyUpdate()
        {
            _sceneDictionary[_sceneType].SceneUpdate();
        }
    }
}
