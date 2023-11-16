using UnityEngine;

namespace MornSound
{
    public readonly struct MornSoundParameter
    {
        // 半音
        public static MornSoundParameter Default => new(1f / 1.05946309f, 1.05946309f, -30f);
        private readonly float _pitchMin;
        private readonly float _pitchMax;
        private readonly float _minDb;

        public MornSoundParameter(float pitchMin, float pitchMax, float minDb)
        {
            _pitchMin = pitchMin;
            _pitchMax = pitchMax;
            _minDb = minDb;
        }

        public float GetRandomPitch()
        {
            return Random.Range(_pitchMin, _pitchMax);
        }

        public float VolumeRateToDecibel(float rate)
        {
            return rate <= 0 ? -5000 : (1 - rate) * _minDb;
        }
    }
}