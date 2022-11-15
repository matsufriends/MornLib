using System;
using UnityEngine;
namespace MornLib.Singletons {
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T> {
        private static T s_instance;
        private bool didAwake;
        public static T Instance {
            get {
                if(s_instance != null) return s_instance;
                s_instance = FindObjectOfType<T>();
                if(s_instance == null) Debug.LogError($"{typeof(T)}が見つかりません");
                s_instance.MyAwake();
                return s_instance;
            }
        }
        private void Awake() {
            if(s_instance == null) {
                s_instance = (T)this;
                DontDestroyOnLoad(gameObject);
                MyAwake();
            } else if(s_instance == this) {
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
        protected abstract void MyAwake();
    }
    public abstract class SingletonMono<TMono,TInterface> : MonoBehaviour where TMono : SingletonMono<TMono,TInterface>,TInterface,ISingleton {
        private static TInterface s_instance;
        public static TInterface Instance {
            get {
                if(s_instance != null) return s_instance;
                var mono = FindObjectOfType<TMono>();
                s_instance = mono;
                if(s_instance == null) throw new Exception($"{typeof(TMono)}が見つかりません");
                mono.Instanced();
                return s_instance;
            }
        }
    }
}
