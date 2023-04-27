namespace MornSound
{
    public interface IMornSoundParameter
    {
        public float BgmChangeSeconds { get; }
        public float GetRandomPitch();
        public float LoadVolumeRate(MornSoundVolumeType volumeType);
        public void SaveVolumeRate(MornSoundVolumeType volumeType, float volume);
        float VolumeRateToDecibel(float rate);
    }
}
