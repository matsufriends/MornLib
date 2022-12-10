using System.IO;
using MornLib.Cores;
using UnityEditor;
using UnityEngine;

namespace MornLib.Singletons
{
    public class MornSingletonSo<T> : ScriptableObject where T : ScriptableObject
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance != null)
                {
                    return s_instance;
                }

                var objects = Resources.FindObjectsOfTypeAll<T>();
                switch (objects.Length)
                {
                    case 0:
                        s_instance = CreateInstance<T>();
#if UNITY_EDITOR
                        var fileName = $"{typeof(T).Name}.asset";
                        var path = Path.Combine("Assets", fileName);
                        AssetDatabase.CreateAsset(s_instance, path);
                        MornLog.Log($"SingletonSo:{fileName}を{path}に作成しました");
#endif
                        break;
                    case 1:
                        s_instance = objects[0];
                        break;
                    default:
                        MornLog.Error($"{typeof(T).Name}が{objects.Length}つ存在します");
                        break;
                }

                return s_instance;
            }
        }
    }
}
