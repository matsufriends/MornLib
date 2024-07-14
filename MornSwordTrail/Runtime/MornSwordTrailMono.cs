using System;
using System.Collections.Generic;
using MornEase;
using UnityEngine;

namespace MornSwordTrail
{
    [ExecuteAlways]
    public sealed class MornSwordTrailMono : MonoBehaviour
    {
        [SerializeField] public MornSwordTrailSettingSo _settings;
        [SerializeField] private Transform _origin;
        [SerializeField] private Transform _swordTop;
        [SerializeField] private Transform _swordBottom;
        private readonly List<TrailInfo> _trailInfoList = new();
        private Vector3 _beforeBottomPos;
        private Vector3 _beforeCenterPos;
        private Color[] _colors;
        private bool _isInitialized;
        private bool _isRegisterTrail;
        private float _lastUpdateTime;
        private Mesh _mesh;
        private float _swordLength;
        private int[] _triangles;
        private Vector2[] _uvs;
        private Vector3[] _vertices;

        private void Start()
        {
            DrawReset();
        }

        private void LateUpdate()
        {
            if (!_settings || !_origin || !_swordTop || !_swordBottom) return;

            if (_isInitialized == false)
            {
                _mesh = new Mesh();
                _vertices = Array.Empty<Vector3>();
                _uvs = Array.Empty<Vector2>();
                _colors = Array.Empty<Color>();
                _triangles = Array.Empty<int>();
                _isInitialized = true;
            }

            _swordLength = (_swordTop.position - _swordBottom.position).magnitude;
            if (_isRegisterTrail || Application.isPlaying == false) RegisterTrails();

            _lastUpdateTime = Time.time;
            for (var i = _trailInfoList.Count - 1; i >= 0; i--)
            {
                _trailInfoList[i].MyUpdate(_lastUpdateTime, _settings.EaseType, out var isDead);
                if (isDead) _trailInfoList.RemoveAt(i);
            }

            //頂点と三角形のリストを更新
            var trailCount = _trailInfoList.Count;
            if (trailCount == 0)
            {
                _mesh.Clear();
                return;
            }

            Array.Resize(ref _vertices, trailCount * 2);
            Array.Resize(ref _uvs, trailCount * 2);
            Array.Resize(ref _colors, trailCount * 2);
            Array.Resize(ref _triangles, (trailCount - 1) * 6);
            for (var i = 0; i < trailCount; i++)
            {
                var trailInfo = _trailInfoList[i];
                _vertices[i * 2] = trailInfo.TopPos;
                _vertices[i * 2 + 1] = trailInfo.BottomPos;
                _uvs[i * 2] = new Vector2((float)i / trailCount, 1);
                _uvs[i * 2 + 1] = new Vector2((float)i / trailCount, 0);
                _colors[i * 2] = trailInfo.TrailColor;
                _colors[i * 2 + 1] = trailInfo.TrailColor;
                if (i == trailCount - 1) break;

                _triangles[i * 6] = i * 2;
                _triangles[i * 6 + 1] = i * 2 + 1;
                _triangles[i * 6 + 2] = i * 2 + 2;
                _triangles[i * 6 + 3] = i * 2 + 2;
                _triangles[i * 6 + 4] = i * 2 + 1;
                _triangles[i * 6 + 5] = i * 2 + 3;
            }

            //Meshに反映
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.colors = _colors;
            _mesh.uv = _uvs;
            _mesh.RecalculateNormals();

            //更新
            var renderParams = new RenderParams(_settings.Material);
            Graphics.RenderMesh(renderParams, _mesh, 0, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one));
        }

        public void DrawReset()
        {
            _beforeBottomPos = _swordBottom.position;
            _beforeCenterPos = (_swordTop.position + _beforeBottomPos) / 2f;
            _lastUpdateTime = Time.time;
            _trailInfoList.Clear();
        }

        public void IsGenerateTrail(bool isRegisterTrail)
        {
            _isRegisterTrail = isRegisterTrail;
            if (isRegisterTrail) DrawReset();
        }

        private void RegisterTrails()
        {
            var topPos = _swordTop.position;
            var bottomPos = _swordBottom.position;
            var originPos = _origin.position;
            var swordCenterPos = (topPos + bottomPos) / 2f;
            var prevOriginToSwordLength = (_beforeBottomPos - originPos).magnitude;
            var nextOriginToSwordLength = (bottomPos - originPos).magnitude;
            var deltaCenterPos = (_beforeCenterPos - swordCenterPos).magnitude;
            var count = Mathf.CeilToInt(deltaCenterPos / Mathf.Max(0.001f, _settings.DrawDistanceThreshold));
            var lerpDif = 1f / Mathf.Max(1, count);
            for (var i = 0; i < count; i++)
            {
                var lerpT = lerpDif * i;
                var originToSwordLength = Mathf.Lerp(prevOriginToSwordLength, nextOriginToSwordLength, lerpT);
                var bottom = Vector3.Lerp(_beforeBottomPos, bottomPos, lerpT);
                bottom = originPos + (bottom - originPos).normalized * originToSwordLength;
                var center = Vector3.Lerp(_beforeCenterPos, swordCenterPos, lerpT);
                var top = bottom + (center - bottom).normalized * _swordLength;
                var spawnTime = Mathf.Lerp(_lastUpdateTime, Time.time, lerpT);
                _trailInfoList.Add(new TrailInfo(top, bottom, _settings.TrailColor, spawnTime, _settings.LifeTime));
            }

            _beforeCenterPos = swordCenterPos;
            _beforeBottomPos = bottomPos;
        }

        private sealed class TrailInfo
        {
            private readonly Vector3 _centerPos;
            private readonly float _lifeTime;
            private readonly float _spawnTime;
            private readonly Vector3 _topDif;

            internal TrailInfo(Vector3 topPos, Vector3 bottomPos, Color trailColor, float spawnTime, float lifeTime)
            {
                TopPos = topPos;
                BottomPos = bottomPos;
                TrailColor = trailColor;
                _centerPos = (topPos + bottomPos) / 2;
                _topDif = topPos - _centerPos;
                _spawnTime = spawnTime;
                _lifeTime = lifeTime;
            }

            public Vector3 TopPos { get; private set; }
            public Vector3 BottomPos { get; private set; }
            public Color TrailColor { get; private set; }

            internal void MyUpdate(float currentTime, MornEaseType easeType, out bool isDead)
            {
                var difTime = currentTime - _spawnTime;
                var rate = (1f - Mathf.Clamp01(difTime / _lifeTime)).Ease(easeType);
                TopPos = _centerPos + _topDif * rate;
                BottomPos = _centerPos - _topDif * rate;
                TrailColor = new Color(TrailColor.r, TrailColor.g, TrailColor.b, rate * TrailColor.a);
                isDead = difTime >= _lifeTime;
            }
        }
    }
}