using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingStringSo), menuName = "MornSetting/" + nameof(MornSettingStringSo))]
    public sealed class MornSettingStringSo : MornSettingSoBase<string>
    {
        protected override string LoadValueImpl()
        {
            return PlayerPrefs.GetString(Key, DefaultValue);
        }

        protected override void SaveValueImpl(string value)
        {
            PlayerPrefs.SetString(Key, value);
        }
    }
}