using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MornLib.Scenes
{
    public abstract class SceneControllerMonoBase<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] private List<SceneMonoBase<TEnum>> _sceneList;
        [SerializeField] private TEnum _sceneType;
        private readonly Dictionary<TEnum, SceneMonoBase<TEnum>> _sceneDictionary = new();

        private void Awake()
        {
            foreach (var scene in _sceneList)
            {
                _sceneDictionary.Add(scene.SceneType, scene);
                scene.OnChangeScene.Subscribe(ChangeScene).AddTo(this);
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

        public void MyUpdate()
        {
            _sceneDictionary[_sceneType].SceneUpdate();
        }
    }
}
