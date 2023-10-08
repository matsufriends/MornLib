using TMPro;
using UniRx;
using UnityEngine;

namespace MornLocalize
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class MornLocalizeTextMono : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private MornLocalizeDataSo _mornLocalizeData;

        private void Awake()
        {
            MornLocalizeCore.OnLanguageChanged.Subscribe(ApplyLanguage).AddTo(this);
        }

        public void ApplyLanguage(MornLocalizeLanguageType languageType)
        {
            if (_mornLocalizeData == null)
            {
                return;
            }

            _text.text = _mornLocalizeData.GetText(languageType);
        }

        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
    }
}