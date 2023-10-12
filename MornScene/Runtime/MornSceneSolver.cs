using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MornScene
{
    [RequireComponent(typeof(MornSceneDictionaryMono))]
    public sealed class MornSceneSolver : MonoBehaviour
    {
        //変数名はEditor拡張よりシリアライズされるので要注意
        [SerializeField] private MornSceneDictionaryMono _sceneDictionary;
        [SerializeField] private MornSceneDataSo _firstScene;
        private readonly Dictionary<MornSceneDataSo, MornSceneMonoBase> _nameToSceneDict = new();
        internal MornSceneMonoBase this[MornSceneDataSo sceneName] => _nameToSceneDict[sceneName];

        private MornSceneController _sceneController;

        private void OnDestroy()
        {
            _sceneController?.Reset();
        }

        private void Reset()
        {
            _sceneDictionary = GetComponent<MornSceneDictionaryMono>();
        }

        private void Awake()
        {
            _sceneController = new MornSceneController(this);
            
            _nameToSceneDict.Clear();
            foreach (var pair in _sceneDictionary.GetDictionary())
            {
                _nameToSceneDict.Add(pair.Key, pair.Value);
                pair.Value.Initialize(pair.Key);
                
                pair.Value.OnChangeSceneRx.Subscribe(_sceneController.ChangeScene);
                pair.Value.OnAddSceneRx.Subscribe(_sceneController.AddScene);
                pair.Value.OnRemoveSceneRx.Subscribe(_sceneController.RemoveScene);
            }
        }

        private void Start()
        {
            foreach (var pair in _sceneDictionary.GetDictionary())
            {
                pair.Value.SetSceneActive(pair.Key, false);
            }

            _sceneController.ChangeScene(_firstScene);
        }

        private void Update()
        {
            _sceneController.UpdateScene();
        }
    }
}