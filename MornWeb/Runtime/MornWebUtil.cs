using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MornLib
{
    public static class MornWebUtil
    {
        [DllImport("__Internal")]
        private static extern void OpenWindow(string url);

        public static void Open(string url)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                OpenWindow(url);
            }
            else
            {
#if UNITY_EDITOR
                Process.Start(url);
#else
				UnityEngine.Debug.Log ("WebGL以外では実行できません。");
				UnityEngine.Debug.Log (url);
#endif
            }
        }
    }
}