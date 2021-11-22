namespace MornLib.Singletons {
    public abstract class Singleton<T> where T : Singleton<T>, new() {
        private static T s_instance;
        protected static T Instance {
            get {
                if (s_instance != null) return s_instance;
                s_instance = new T();
                s_instance.Instanced();
                return s_instance;
            }
        }
        protected abstract void Instanced();
    }
    public abstract class Singleton<TClass, TInterface> where TInterface : ISingleton where TClass : Singleton<TClass, TInterface>, TInterface, new() {
        private static TInterface s_instance;
        public static TInterface Instance {
            get {
                if (s_instance != null) return s_instance;
                s_instance = new TClass();
                s_instance.Instanced();
                return s_instance;
            }
        }
    }
}