using UnityEngine;

namespace MornBeat
{
    public sealed class MornBeatScalerMono : MonoBehaviour
    {
        [SerializeField] private MornBeatScalerSettingSo _mornBeatScalerSetting;
        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = transform.localScale;
            /*
            MornBeatCore.OnBeat.Where(beat => beat.IsJustForAnyBeat(4)).Subscribe(
                _ =>
                {
                    transform.localScale = _defaultScale * _mornBeatScalerSetting.AimScale;
                }).AddTo(this);*/
        }

        private void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _defaultScale, _mornBeatScalerSetting.ScaleLerpT * Time.deltaTime);
        }
    }
}