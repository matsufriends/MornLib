using MornSetting;
using UniRx;
using UnityEngine;

namespace MornBeat
{
    internal sealed class MornBeatSolverMono : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private MornSettingFloatSo _mornTimingSetting;
        [SerializeField] private float _timingScaleK = 0.05f;
        private static MornBeatSolverMono s_instance;
        internal static MornBeatSolverMono Instance
        {
            get
            {
                if (s_instance != null)
                {
                    return s_instance;
                }

                s_instance = FindObjectOfType<MornBeatSolverMono>();
                if (s_instance == null)
                {
                    Debug.LogError($"{nameof(MornBeatSolverMono)} is not found.");
                }

                return s_instance;
            }
        }

        private void Awake()
        {
            MornBeatCore.OffsetTime = _mornTimingSetting.LoadFloat() * _timingScaleK;
            _mornTimingSetting.OnFloatChanged.Subscribe(
                x =>
                {
                    MornBeatCore.OffsetTime = x * _timingScaleK;
                }).AddTo(this);
        }

        internal void OnInitializeBeat(MornBeatMemoSo beatMemo, double dspTime)
        {
            _audioSource.loop = beatMemo.IsLoop;
            _audioSource.clip = beatMemo.Clip;
            _audioSource.PlayScheduled(dspTime);
        }

        private void OnDestroy()
        {
            MornBeatCore.Reset();
        }
    }
}
