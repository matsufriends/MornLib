using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingIntSo), menuName = "MornSetting/" + nameof(MornSettingIntSo))]
    public sealed class MornSettingIntSo : MornSettingSoBase<int>
    {
        protected override int LoadValueImpl()
        {
            return PlayerPrefs.GetInt(Key, DefaultValue);
        }

        protected override void SaveValueImpl(int value)
        {
            PlayerPrefs.SetInt(Key, value);
        }
    }
}