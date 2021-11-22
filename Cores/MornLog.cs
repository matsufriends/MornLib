using System;
using UnityEngine;
namespace MornLib.Cores {
    public static class MornLog {
        public static void Warning(string text) {
            Debug.LogWarning(text);
        }
        public static void Error(string text) {
            throw new Exception(text);
        }
    }
}