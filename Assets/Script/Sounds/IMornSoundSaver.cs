namespace MornLib.Sounds
{
    public interface IMornSoundSaver
    {
        public float LoadVolume(MornSoundVolumeType volumeType, float defaultVolume);
        public void SaveVolume(MornSoundVolumeType volumeType, float volume);
    }
}
