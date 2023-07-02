using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace MornScene
{
    public abstract class MornSceneSolverBase<TEnum> : MonoBehaviour where TEnum : Enum
    {
        //変数名はEditor拡張よりシリアライズされるので要注意
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private TEnum _firstSceneType;
        [SerializeField] protected List<TEnum> _keyList;
        [SerializeField] protected List<MornSceneMonoBase> _valueList;
        private readonly Dictionary<TEnum, MornSceneMonoBase> _typeToSceneDict = new();
        private readonly Dictionary<MornSceneMonoBase, TEnum> _sceneToTypeDict = new();
        private static MornSceneSolverBase<TEnum> s_instance;

        internal MornSceneMonoBase this[TEnum sceneType] => _typeToSceneDict[sceneType];

        internal TEnum this[MornSceneMonoBase scene] => _sceneToTypeDict[scene];

        public static MornSceneSolverBase<TEnum> Instance
        {
            get
            {
                if (s_instance != null)
                {
                    return s_instance;
                }

                s_instance = FindObjectOfType<MornSceneSolverBase<TEnum>>();
                if (s_instance == null)
                {
                    Debug.LogError($"{nameof(MornSceneSolverBase<TEnum>)} is not found.");
                }

                return s_instance;
            }
        }

        private void OnDestroy()
        {
            MornSceneCore.Reset();
        }

        //関数名がEditor拡張より指定されているため要注意
        public void ApplyCanvasScale()
        {
            foreach (var scene in _valueList.Where(scene => scene != null))
            {
                scene.ApplyCanvasScale(_width, _height);
            }

            Debug.Log("Canvas Scale Applied.");
        }

        //関数名がEditor拡張より指定されているため要注意
        public void HideAll()
        {
            Assert.IsTrue(_keyList.Count == _valueList.Count);
            for (var i = 0; i < _valueList.Count; i++)
            {
                var sceneType = _keyList[i];
                var scene = _valueList[i];
                if (scene != null)
                {
                    scene.SetSceneActive(sceneType, false);
                }
            }

            Debug.Log("All Scenes Hidden.");
        }

        //関数名がEditor拡張より指定されているため要注意
        public void ChangeScene(string sceneName)
        {
            Assert.IsTrue(_keyList.Count == _valueList.Count);
            for (var i = 0; i < _valueList.Count; i++)
            {
                var sceneType = _keyList[i];
                var scene = _valueList[i];
                if (scene != null)
                {
                    scene.SetSceneActive(sceneType, sceneType.ToString() == sceneName);
                }
            }

            Debug.Log($"Scene Changed to {sceneName}.");
        }

        private void Awake()
        {
            _typeToSceneDict.Clear();
            _sceneToTypeDict.Clear();
            for (var i = 0; i < _keyList.Count; i++)
            {
                var sceneType = _keyList[i];
                var scene = _valueList[i];
                _typeToSceneDict.Add(sceneType, scene);
                _sceneToTypeDict.Add(scene, sceneType);
            }

            foreach (var scene in _valueList)
            {
                scene.Initialize();
            }
        }

        private void Start()
        {
            MornSceneCore.ChangeScene(_firstSceneType);
        }

        private void Update()
        {
            MornSceneCore.UpdateScene();
        }
    }
}
