using System;
using UnityEngine;

namespace MornLib.Sounds
{
    public class MornSoundSaver : IMornSoundSaver
    {
        private const string MasterVolumeKey = "MasterVolume";
        private const string SeVolumeKey = "SeVolume";
        private const string BGMVolumeKey = "BgmVolume";

        public float LoadVolume(MornSoundVolumeType volumeType, float defaultVolume)
        {
            return PlayerPrefs.GetFloat(MornSoundSlideTypeToKey(volumeType), defaultVolume);
        }

        public void SaveVolume(MornSoundVolumeType volumeType, float volume)
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
    }
}
