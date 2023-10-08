using System;
using UnityEngine;

namespace MornLocalize
{
    [CreateAssetMenu(fileName = nameof(MornLocalizeDataSo), menuName = "MornLib/" + nameof(MornLocalizeDataSo), order = 0)]
    public sealed class MornLocalizeDataSo : ScriptableObject
    {
        [SerializeField, Multiline] private string _japanese;
        [SerializeField, Multiline] private string _english;

        public string GetText(MornLocalizeLanguageType mornLocalizeLanguage)
        {
            return mornLocalizeLanguage switch
            {
                MornLocalizeLanguageType.Japanese => _japanese,
                MornLocalizeLanguageType.English  => _english,
                _                                 => throw new ArgumentOutOfRangeException(nameof(mornLocalizeLanguage), mornLocalizeLanguage, null)
            };
        }
    }
}