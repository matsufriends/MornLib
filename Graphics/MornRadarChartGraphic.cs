using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MornLib.Graphics
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasRenderer))]
    public class MOrnRadarChartGraphic : Graphic
    {
        [SerializeField] private Color _lineColor;
        [SerializeField] private List<float> _valueList;
        [SerializeField] private bool _drawInside;
        [SerializeField] private float _lineWidth;
        [SerializeField] private float _scaleCount;
        [SerializeField] private float _splitWidth;
        private int _valueCount;
        private float _baseRadius;
        private RectTransform _rectTransform;

        protected override void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            _valueCount = _valueList.Count;
            var size = _rectTransform.sizeDelta;
            _baseRadius = Mathf.Min(size.x, size.y) / 2;
            if (_drawInside)
            {
                GenerateInsideMesh(vh);
            }

            if (0 < _lineWidth && 1 <= _scaleCount)
            {
                var dif = 1f / _scaleCount;
                for (var i = dif; i <= 1; i += dif)
                {
                    GenerateBroadLine(vh, i);
                }
            }

            if (_splitWidth > 0)
            {
                GenerateSplitLine(vh);
            }
        }

        private void GenerateInsideMesh(VertexHelper vh)
        {
            vh.AddVert(GenerateUIVertex(GetPos(0), color)); //Center
            for (var i = 0; i < _valueCount; i++)
            {
                vh.AddVert(GenerateUIVertex(GetPos(i) * _baseRadius * _valueList[i], color)); //Per Vertex
            }

            for (var i = 0; i < _valueCount; i++)
            {
                vh.AddTriangle(0, i + 1, (i + 1) % _valueCount + 1);
            }
        }

        private void GenerateBroadLine(VertexHelper vh, float k)
        {
            var dif = _lineWidth / 2f;
            var baseCount = vh.currentVertCount;
            for (var i = 0; i < _valueCount; i++)
            {
                vh.AddVert(GenerateUIVertex(GetPos(i) * (_baseRadius * k * _valueList[i] + dif),
                    _lineColor)); //Outer Vertex
                vh.AddVert(GenerateUIVertex(GetPos(i) * (_baseRadius * k * _valueList[i] - dif),
                    _lineColor)); //Inner Vertex
            }

            for (var i = 0; i < _valueCount; i++)
            {
                vh.AddTriangle(baseCount + 2 * i, baseCount + (2 * i + 2) % (_valueCount * 2),
                    baseCount + (2 * i + 3) % (_valueCount * 2));
                vh.AddTriangle(baseCount + 2 * i, baseCount + (2 * i + 3) % (_valueCount * 2),
                    baseCount + (2 * i + 1) % (_valueCount * 2));
            }
        }

        private void GenerateSplitLine(VertexHelper vh)
        {
            var baseCount = vh.currentVertCount;
            for (var i = 0; i < _valueCount; i++)
            {
                var normal = GetNormal(i) * _splitWidth;
                var outerPos = GetPos(i) * _baseRadius * _valueList[i];
                vh.AddVert(GenerateUIVertex(outerPos + normal, _lineColor));
                vh.AddVert(GenerateUIVertex(Vector2.zero + normal, _lineColor));
                vh.AddVert(GenerateUIVertex(outerPos - normal, _lineColor));
                vh.AddVert(GenerateUIVertex(Vector2.zero - normal, _lineColor));
                vh.AddTriangle(baseCount + i * 4, baseCount + i * 4 + 2, baseCount + i * 4 + 3);
                vh.AddTriangle(baseCount + i * 4, baseCount + i * 4 + 3, baseCount + i * 4 + 1);
            }
        }

        private UIVertex GenerateUIVertex(Vector2 pos, Color vertexColor)
        {
            var result = UIVertex.simpleVert;
            result.position = pos;
            result.color = vertexColor;
            return result;
        }

        private Vector2 GetPos(int index)
        {
            var radian = Mathf.Deg2Rad * (90 - index * 360 / Mathf.Max(1, _valueCount));
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        private Vector2 GetNormal(int index)
        {
            var radian = Mathf.Deg2Rad * (90 - index * 360 / Mathf.Max(1, _valueCount));
            return new Vector2(-Mathf.Sin(radian), Mathf.Cos(radian));
        }

        [ContextMenu("UpdateMesh")]
        public void UpdateMesh()
        {
            SetAllDirty();
        }

        public void SetValue(IEnumerable<float> values)
        {
            _valueList.Clear();
            _valueList.AddRange(values);
            UpdateMesh();
        }
    }
}
