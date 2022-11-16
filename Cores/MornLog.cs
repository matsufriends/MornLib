using UnityEngine;
namespace MornLib.Cores {
    public static class MornLog {
        public static void Log(string text) => Debug.Log(text);
        public static void Warning(string text) => Debug.LogWarning(text);
        public static void Error(string text) => Debug.LogError(text);
    }
}
