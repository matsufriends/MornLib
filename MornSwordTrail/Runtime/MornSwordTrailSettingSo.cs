using MornEase;
using UnityEngine;

namespace MornSwordTrail
{
    [CreateAssetMenu(fileName = nameof(MornSwordTrailSettingSo), menuName = "MornLib/" + nameof(MornSwordTrailSettingSo))]
    public sealed class MornSwordTrailSettingSo : ScriptableObject
    {
        [SerializeField] private Material _material;
        [SerializeField] private Color _trailColor = Color.white;
        [SerializeField] private float _drawDistanceThreshold = 0.01f;
        [SerializeField] private float _lifeTime = 0.3f;
        [SerializeField] private MornEaseType _easeType = MornEaseType.EaseInSine;
        public Material Material => _material;
        public Color TrailColor => _trailColor;
        public float DrawDistanceThreshold => _drawDistanceThreshold;
        public float LifeTime => _lifeTime;
        public MornEaseType EaseType => _easeType;
    }
}
