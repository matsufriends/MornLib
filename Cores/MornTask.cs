using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace MornLib.Cores {
    public static class MornTask {
        public static async UniTask TransitionAsync(float time,bool isUnscaledTime,Action<float> action) {
            if(time <= 0) throw new ArgumentException($"time[{time}]は0以上の値を指定して下さい。");
            var startTime = isUnscaledTime ? Time.unscaledTime : Time.time;
            while(true) {
                var dif = (isUnscaledTime ? Time.unscaledTime : Time.time) - startTime;
                action(Mathf.Clamp01(dif / time));
                if(dif >= time) return;
                await UniTask.NextFrame();
            }
        }
    }
}