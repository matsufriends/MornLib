using UnityEngine;

namespace MornSound
{
    internal sealed class MornSoundParameter : IMornSoundParameter
    {
        private const float BgmChangeSeconds = 1;
        private const float RandomPitchMin = 1f / RandomPitchRate;
        private const float RandomPitchMax = 1f * RandomPitchRate;
        private const float RandomPitchRate = 1.05946309f; //半音
        private const float MinDb = 30;
        float IMornSoundParameter.BgmChangeSeconds => BgmChangeSeconds;

        float IMornSoundParameter.GetRandomPitch()
        {
            return Random.Range(RandomPitchMin, RandomPitchMax);
        }

        float IMornSoundParameter.VolumeRateToDecibel(float rate)
        {
            return rate <= 0 ? -5000 : (rate - 1) * MinDb;
        }
    }
}
