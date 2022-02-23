using System;
namespace MornLib.Cores {
    public static class MornLog {
        public static void Warning(string text) {
            #if UNITY_EDITOR
            throw new Exception(text);
            #else
            Debug.LogWarning(text);
            #endif
        }
        public static void Error(string text) {
            throw new Exception(text);
        }
    }
}