namespace MornSound
{
    public interface IMornSoundParameter
    {
        public float GetRandomPitch();
        float VolumeRateToDecibel(float rate);
    }
}