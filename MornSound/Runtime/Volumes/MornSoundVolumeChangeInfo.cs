namespace MornSound
{
    public readonly struct MornSoundVolumeChangeInfo
    {
        public readonly MornSoundVolumeType VolumeType;
        public readonly float VolumeRate;
        public readonly float VolumeDecibel;

        public MornSoundVolumeChangeInfo(MornSoundVolumeType volumeType, float volumeRate, float volumeDecibel)
        {
            VolumeType = volumeType;
            VolumeRate = volumeRate;
            VolumeDecibel = volumeDecibel;
        }
    }
}
