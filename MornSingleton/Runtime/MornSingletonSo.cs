using System.IO;
using UnityEditor;
using UnityEngine;

namespace MornSingleton
{
    public class MornSingletonSo<T> : ScriptableObject where T : ScriptableObject
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance != null) return s_instance;

                s_instance = Resources.Load<T>(typeof(T).Name);
                if (s_instance != null) return s_instance;

                s_instance = CreateInstance<T>();
#if UNITY_EDITOR
                var fileName = $"{typeof(T).Name}.asset";
                var path = Path.Combine("Assets/Resources", fileName);
                AssetDatabase.CreateAsset(s_instance, path);
                Debug.Log($"SingletonSo:{fileName}を{path}に作成しました");
#endif
                return s_instance;
            }
        }
    }
}