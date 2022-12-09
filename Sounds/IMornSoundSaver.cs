namespace MornLib.Sounds
{
    public interface IMornSoundSaver
    {
        public float LoadVolume(MornSoundSliderType sliderType, float defaultVolume);
        public void SaveVolume(MornSoundSliderType sliderType, float volume);
    }
}
