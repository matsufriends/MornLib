using UnityEditor;

namespace MornLib.Editor
{
    [FilePath("MornHierarchySettings.asset", FilePathAttribute.Location.PreferencesFolder)]
    public class MornHierarchySettings : ScriptableSingleton<MornHierarchySettings>
    {
        public bool ShowTag;

        public void Save()
        {
            Save(true);
        }
    }
}
