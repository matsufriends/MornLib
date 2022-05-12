/*
using Sirenix.OdinInspector;
using UnityEngine;
namespace MornLib.Singletons {
    //     Odin未使用の場合はこのファイル自体を削除してください
    public abstract class OdinSingletonMono<T> : SerializedMonoBehaviour where T : OdinSingletonMono<T> {
        private static T s_instance;
        public static T Instance {
            get {
                if(s_instance != null) return s_instance;
                s_instance = FindObjectOfType<T>();
                if(s_instance == null) Debug.LogError($"{typeof(T)}が見つかりません");
                return s_instance;
            }
        }
    }
    public abstract class SingletonSerializedMono<TMono,TInterface> : SerializedMonoBehaviour
        where TInterface : class where TMono : SingletonSerializedMono<TMono,TInterface>,TInterface {
        private static TInterface s_instance;
        public static TInterface Instance {
            get {
                if(s_instance != null) return s_instance;
                s_instance = FindObjectOfType<TMono>();
                if(s_instance == null) Debug.LogError($"{typeof(TMono)}が見つかりません");
                return s_instance;
            }
        }
    }
}
*/