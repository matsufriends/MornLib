using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingBoolSo), menuName = "MornSetting/" + nameof(MornSettingBoolSo))]
    public sealed class MornSettingBoolSo : MornSettingSoBase<bool>
    {
        protected override bool LoadValueImpl()
        {
            var value = PlayerPrefs.GetInt(Key, -1);
            return value < 0 ? DefaultValue : value > 0;
        }

        protected override void SaveValueImpl(bool value)
        {
            PlayerPrefs.SetInt(Key, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}