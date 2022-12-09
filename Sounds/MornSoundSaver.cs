using System;
using UnityEngine;

namespace MornLib.Sounds
{
    public class MornSoundSaver : IMornSoundSaver
    {
        private const string MasterVolumeKey = "MasterVolume";
        private const string SeVolumeKey = "SeVolume";
        private const string BGMVolumeKey = "BgmVolume";

        public float LoadVolume(MornSoundSliderType sliderType,float defaultVolume)
        {
            return PlayerPrefs.GetFloat(MornSoundSlideTypeToKey(sliderType), defaultVolume);
        }

        public void SaveVolume(MornSoundSliderType sliderType, float volume)
        {
            PlayerPrefs.SetFloat(MornSoundSlideTypeToKey(sliderType), volume);
            PlayerPrefs.Save();
        }

        private static string MornSoundSlideTypeToKey(MornSoundSliderType sliderType)
        {
            return sliderType switch
            {
                MornSoundSliderType.Master => MasterVolumeKey,
                MornSoundSliderType.Se => SeVolumeKey,
                MornSoundSliderType.Bgm => BGMVolumeKey,
                _ => throw new ArgumentOutOfRangeException(nameof(sliderType), sliderType, null),
            };
        }
    }
}
