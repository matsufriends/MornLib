using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace MornLib.Frames
{
    public class MornFrameCounterMono : MonoBehaviour
    {
        [SerializeField] private float _log;
        [SerializeField] private int _saveFrames;
        private readonly Queue<float> _fpsQueue = new();
        private float _cachedUpdateTime;

        private void Awake()
        {
            MornFrameManagerMono.Instance.OnUpdate.Subscribe(_ =>
                {
                    var cur = Time.realtimeSinceStartup;
                    var fps = 1 / (cur - _cachedUpdateTime);
                    _fpsQueue.Enqueue(fps);
                    if (_fpsQueue.Count > Mathf.Max(1, _saveFrames))
                    {
                        _fpsQueue.Dequeue();
                    }

                    _log = _fpsQueue.Average();
                    _cachedUpdateTime = cur;
                })
                .AddTo(this);
        }
    }
}
