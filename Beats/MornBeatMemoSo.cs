using System;
using System.Collections.Generic;
using MornLib.Cores;
using UnityEditor;
using UnityEngine;

namespace MornLib.Beats
{
    [CreateAssetMenu(fileName = nameof(MornBeatMemoSo), menuName = nameof(MornBeatMemoSo))]
    public sealed class MornBeatMemoSo : ScriptableObject
    {
        [Serializable]
        private struct BpmAndTimeInfo
        {
            public double Bpm;
            public double Time;
        }

        [SerializeField] private List<float> _timingList;
        [SerializeField] private List<BpmAndTimeInfo> _bpmAndTimeInfoList;
        [SerializeField] private int _aimTimingCount = 128;
        [SerializeField] private int _beatCount = 4;
        [SerializeField] private double _interval = 0.000001d;
        public int Timings => _timingList.Count;

        public float GetBeatTiming(int index)
        {
            if (index < 0 || Timings <= index)
            {
                return Mathf.Infinity;
            }

            return _timingList[index];
        }

        public void MakeBeat()
        {
            var beat = 0d;
            var time = 0d;
            _interval = Math.Max(0.000001f, _interval);
            _timingList.Clear();
            while (beat <= _aimTimingCount)
            {
                var bpm = GetBpm(time);
                var dif = bpm / 60 * _beatCount / 4 * _interval;
                if (Math.Floor(beat) < Math.Floor(beat + dif))
                {
                    _timingList.Add((float)time);
                }

                beat += dif;
                time += _interval;
            }
        }

        private double GetBpm(double time)
        {
            switch (_bpmAndTimeInfoList.Count)
            {
                case 0:
                    return 60;
                case 1:
                    return _bpmAndTimeInfoList[0].Bpm;
            }

            if (time < _bpmAndTimeInfoList[0].Time)
            {
                return _bpmAndTimeInfoList[0].Bpm;
            }

            for (var i = 1; i < _bpmAndTimeInfoList.Count; i++)
            {
                if (_bpmAndTimeInfoList[i].Time <= time)
                {
                    continue;
                }

                var begin = _bpmAndTimeInfoList[i - 1];
                var end = _bpmAndTimeInfoList[i];
                var t1 = MornMath.InverseLerp(begin.Time, end.Time, time);
                return MornMath.Lerp(begin.Bpm, end.Bpm, t1);
            }

            return _bpmAndTimeInfoList[^1].Bpm;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MornBeatMemoSo))]
    public class BeatMemoSoEditor : Editor
    {
        private MornBeatMemoSo _mornBeatMemo;

        private void OnEnable()
        {
            _mornBeatMemo = (MornBeatMemoSo)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("MakeBeat"))
            {
                _mornBeatMemo.MakeBeat();
            }
        }
    }
#endif
}
