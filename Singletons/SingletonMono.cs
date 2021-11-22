using System;
using UnityEngine;
namespace MornLib.Singletons {
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T> {
        private static T s_instance;
        public static T Instance {
            get {
                if (s_instance != null) return s_instance;
                s_instance = FindObjectOfType<T>();
                if (s_instance == null) Debug.LogError($"{typeof(T)}が見つかりません");
                return s_instance;
            }
        }
    }
    public abstract class SingletonMono<TMono, TInterface> : MonoBehaviour where TInterface : class where TMono : SingletonMono<TMono, TInterface>, TInterface {
        private static TInterface s_instance;
        public static TInterface Instance {
            get {
                if (s_instance != null) return s_instance;
                s_instance = FindObjectOfType<TMono>();
                if (s_instance == null) throw new Exception($"{typeof(TMono)}が見つかりません");
                return s_instance;
            }
        }
    }
}