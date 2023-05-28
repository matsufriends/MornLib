namespace MornSound
{
    public interface IMornSoundParameter
    {
        public float BgmChangeSeconds { get; }
        public float GetRandomPitch();
        float VolumeRateToDecibel(float rate);
    }
}
