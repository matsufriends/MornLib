using TMPro;
using UnityEngine;

namespace MornLib.UIs
{
    public class MornFlashTextMono : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Update()
        {
            var duration = MornFlashSo.Instance.Interval;
            var color = _text.color;
            color.a = 1f - Mathf.PingPong(Time.time, duration) / duration;
            _text.color = color;
        }
    }
}
