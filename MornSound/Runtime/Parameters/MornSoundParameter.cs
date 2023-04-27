using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MornSound
{
    internal sealed class MornSoundParameter : IMornSoundParameter
    {
        private const string MasterVolumeKey = "MasterVolume";
        private const string SeVolumeKey = "SeVolume";
        private const string BGMVolumeKey = "BgmVolume";
        private const float BgmChangeSeconds = 1;
        private const float RandomPitchMin = 1f / RandomPitchRate;
        private const float RandomPitchMax = 1f * RandomPitchRate;
        private const float RandomPitchRate = 1.05946309f; //半音
        private const float DefaultMasterVolume = 0.65f;
        private const float DefaultBgmVolume = 1;
        private const float DefaultSeVolume = 1;
        private const float MinDb = 30;
        float IMornSoundParameter.BgmChangeSeconds => BgmChangeSeconds;

        float IMornSoundParameter.GetRandomPitch()
        {
            return Random.Range(RandomPitchMin, RandomPitchMax);
        }

        float IMornSoundParameter.LoadVolumeRate(MornSoundVolumeType volumeType)
        {
            var defaultVolume = volumeType switch
            {
                MornSoundVolumeType.Master => DefaultMasterVolume,
                MornSoundVolumeType.Se => DefaultSeVolume,
                MornSoundVolumeType.Bgm => DefaultBgmVolume,
                _ => throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null),
            };
            return PlayerPrefs.GetFloat(MornSoundSlideTypeToKey(volumeType), defaultVolume);
        }

        void IMornSoundParameter.SaveVolumeRate(MornSoundVolumeType volumeType, float volume)
        {
            PlayerPrefs.SetFloat(MornSoundSlideTypeToKey(volumeType), volume);
            PlayerPrefs.Save();
        }

        private static string MornSoundSlideTypeToKey(MornSoundVolumeType volumeType)
        {
            return volumeType switch
            {
                MornSoundVolumeType.Master => MasterVolumeKey,
                MornSoundVolumeType.Se => SeVolumeKey,
                MornSoundVolumeType.Bgm => BGMVolumeKey,
                _ => throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null),
            };
        }

        float IMornSoundParameter.VolumeRateToDecibel(float rate)
        {
            return rate <= 0 ? -5000 : (rate - 1) * MinDb;
        }
    }
}
