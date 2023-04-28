using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MornFPS
{
    public sealed class MornFPSCheckerMono : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fpsText;
        [SerializeField] private int _saveFrames = 100;
        private readonly Queue<float> _fpsQueue = new();
        private float _cachedUpdateTime;

        private void Update()
        {
            var cur = Time.realtimeSinceStartup;
            var fps = 1 / (cur - _cachedUpdateTime);
            _fpsQueue.Enqueue(fps);
            if (_fpsQueue.Count > Mathf.Max(1, _saveFrames))
            {
                _fpsQueue.Dequeue();
            }

            _fpsText.text = $"FPS:{_fpsQueue.Average():.00}";
            _cachedUpdateTime = cur;
        }
    }
}
