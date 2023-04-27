using System;
using UnityEngine;

namespace MornSound
{
    [Serializable]
    public readonly struct MornSoundInfo
    {
        public readonly AudioClip AudioClip;
        public readonly bool IsRandomPitch;

        public MornSoundInfo(AudioClip audioClip, bool isRandomPitch)
        {
            AudioClip = audioClip;
            IsRandomPitch = isRandomPitch;
        }
    }
}
