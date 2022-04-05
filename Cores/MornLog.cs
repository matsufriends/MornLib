using System;
using UnityEngine;
namespace MornLib.Cores {
    public static class MornLog {
        public static void Log(string text) {
            Debug.Log(text);
        }
        public static void Warning(string text) {
            #if UNITY_EDITOR
            Debug.LogError(text);
            #else
            Debug.LogWarning(text);
            #endif
        }
        public static void Error(string text) {
            throw new Exception(text);
        }
    }
}