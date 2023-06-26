using System;
using MornDictionary;
using UnityEngine;

namespace MornBeat
{
    public abstract class MornBeatSolverMonoBase<TBeatType> : MonoBehaviour where TBeatType : Enum
    {
        [Header("MakeBeat")] [SerializeField] private MornDictionary<TBeatType, MornBeatMemoSo> _beatDictionary;
        private static MornBeatSolverMonoBase<TBeatType> s_instance;

        internal MornBeatMemoSo this[TBeatType beatType] => _beatDictionary[beatType];

        internal static MornBeatSolverMonoBase<TBeatType> Instance
        {
            get
            {
                if (s_instance != null)
                {
                    return s_instance;
                }

                s_instance = FindObjectOfType<MornBeatSolverMonoBase<TBeatType>>();
                if (s_instance == null)
                {
                    Debug.LogError($"{nameof(MornBeatSolverMonoBase<TBeatType>)} is not found.");
                }

                return s_instance;
            }
        }

        internal void OnInitializeBeatImpl(TBeatType beatType, double dspTime)
        {
            OnInitializedBeat(beatType, _beatDictionary[beatType].Clip, dspTime, _beatDictionary[beatType].IsLoop);
        }

        private void OnDestroy()
        {
            MornBeatCore.Reset();
        }

        protected abstract void OnInitializedBeat(TBeatType beatType, AudioClip clip, double dspTime, bool isLoop);
    }
}
