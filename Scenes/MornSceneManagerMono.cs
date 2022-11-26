using System;
using System.Collections.Generic;
using MornLib.Singletons;
using UnityEngine;

namespace MornLib.Scenes
{
    public class MornSceneManagerMono : SingletonMono<MornSceneManagerMono>
    {
        [SerializeField] private List<MornSceneMono> _scenePrefabList;
        private readonly Dictionary<Type, MornSceneMono> _sceneDictionary = new();

        protected override void MyAwake()
        {
            foreach (var prefab in _scenePrefabList)
            {
                _sceneDictionary.Add(prefab.GetType(), prefab);
            }
        }

        public void GenerateScene(Type sceneType)
        {
            if (sceneType.IsSubclassOf(typeof(MornSceneMono)) == false)
            {
                throw new Exception($"[{sceneType}]は{typeof(MornSceneMono)}を継承していません");
            }

            Instantiate(_sceneDictionary[sceneType], transform);
        }
    }
}