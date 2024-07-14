using System;
using UnityEngine;

namespace MornScene
{
    [Serializable]
    public class MornSceneObject
    {
        [SerializeField] private string _sceneName;

        public static implicit operator string(MornSceneObject mornSceneObject)
        {
            return mornSceneObject._sceneName;
        }

        public static implicit operator MornSceneObject(string sceneName)
        {
            return new MornSceneObject
            {
                _sceneName = sceneName
            };
        }
    }
}