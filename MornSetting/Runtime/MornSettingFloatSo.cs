using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingFloatSo), menuName = "MornSetting/" + nameof(MornSettingFloatSo))]
    public sealed class MornSettingFloatSo : MornSettingSoBase<float>
    {
        protected override float LoadValueImpl()
        {
            return PlayerPrefs.GetFloat(Key, DefaultValue);
        }

        protected override void SaveValueImpl(float value)
        {
            PlayerPrefs.SetFloat(Key, value);
            PlayerPrefs.Save();
        }
    }
}