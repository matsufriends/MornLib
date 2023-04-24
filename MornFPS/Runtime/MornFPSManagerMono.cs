using System.Collections;
using System.Threading;
using UnityEngine;

namespace MornFPS
{
    public sealed class MornFPSManagerMono : MonoBehaviour
    {
        private MornFPSManagerMono _instance;
        [SerializeField] private int _aimFps = 60;
        private float _currentFrameTime;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }

            _instance = this;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 9999;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            StartCoroutine(WaitForNextFrame());
        }

        private IEnumerator WaitForNextFrame()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                _currentFrameTime += 1f / Mathf.Max(1, _aimFps);
                var realTime = Time.realtimeSinceStartup;

                //フレーム秒以上にリアルタイムが進んでいるとき、フレーム秒を補正
                if (_currentFrameTime < realTime)
                {
                    _currentFrameTime = realTime;
                }

                //フレーム秒まで0.01秒以上残っていれば、フレーム秒の0.01秒前までThread.Sleep
                if (realTime + 0.01f < _currentFrameTime)
                {
                    var sleepTime = _currentFrameTime - realTime - 0.01f;
                    Thread.Sleep((int)(sleepTime * 1000));
                }

                //フレーム秒まで待機(0.01秒未満)
                while (realTime < _currentFrameTime)
                {
                    realTime = Time.realtimeSinceStartup;
                }
            }
        }
    }
}
