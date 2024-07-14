using UnityEngine;

namespace MornLib.Mono
{
    public class MornCircleLayoutMono : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _radius;
        [SerializeField] private bool _clock;
        [SerializeField] private float _angleOffset;
        [SerializeField] private float _rotOffset;

        private void OnValidate()
        {
            var count = transform.childCount;
            var degree = 360f / count * (_clock ? -1 : 1);
            var angle = degree * Mathf.Deg2Rad;
            var angleOffset = _angleOffset * Mathf.Deg2Rad;
            for (var i = 0; i < count; i++)
            {
                var r = _curve.Evaluate(i * 1f / count);
                var child = transform.GetChild(i);
                child.localPosition =
                    new Vector2(Mathf.Cos(angleOffset + angle * i * r), Mathf.Sin(angleOffset + angle * i * r)) *
                    _radius;
                child.eulerAngles = new Vector3(0, 0, _rotOffset + degree * i * r);
            }
        }
    }
}