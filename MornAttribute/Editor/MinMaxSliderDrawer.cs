using UnityEditor;
using UnityEngine;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public sealed class MinMaxSliderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var minMaxSlider = (MinMaxSliderAttribute)attribute;
            if (property.propertyType != SerializedPropertyType.Vector2 && property.propertyType != SerializedPropertyType.Vector2Int)
            {
                EditorGUI.HelpBox(position, $"Property type must be Vector2(Int): {property.propertyType}", MessageType.Error);
                return;
            }

            if (minMaxSlider.Min > minMaxSlider.Max)
            {
                EditorGUI.HelpBox(position, $"Min must be less than Max: {minMaxSlider.Min} > {minMaxSlider.Max}", MessageType.Error);
                return;
            }

            var isInt = property.propertyType == SerializedPropertyType.Vector2Int;
            var rect = EditorGUI.PrefixLabel(position, label);
            var width = rect.width;
            var minRect = new Rect(rect.x, rect.y, width * 0.15f, rect.height);
            var sliderRect = new Rect(rect.x + width * 0.2f, rect.y, width * 0.6f, rect.height);
            var maxRect = new Rect(rect.x + width * 0.85f, rect.y, width * 0.15f, rect.height);
            if (isInt)
            {
                var min = property.vector2IntValue.x;
                var max = property.vector2IntValue.y;
                if (min < minMaxSlider.Min || max > minMaxSlider.Max || max < min)
                {
                    min = Mathf.Max(min, Mathf.RoundToInt(minMaxSlider.Min));
                    max = Mathf.Min(max, Mathf.RoundToInt(minMaxSlider.Max));
                    max = Mathf.Max(max, min);
                    property.vector2IntValue = new Vector2Int(min, max);
                }

                EditorGUI.BeginChangeCheck();
                var minF = (float)min;
                var maxF = (float)max;
                EditorGUI.MinMaxSlider(sliderRect, ref minF, ref maxF, minMaxSlider.Min, minMaxSlider.Max);
                min = Mathf.RoundToInt(minF);
                max = Mathf.RoundToInt(maxF);
                min = EditorGUI.IntField(minRect, min);
                max = EditorGUI.IntField(maxRect, max);
                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2IntValue = new Vector2Int(min, max);
                }
            }
            else
            {
                var min = property.vector2Value.x;
                var max = property.vector2Value.y;
                if (min < minMaxSlider.Min || max > minMaxSlider.Max || max < min)
                {
                    min = Mathf.Max(min, minMaxSlider.Min);
                    max = Mathf.Min(max, minMaxSlider.Max);
                    max = Mathf.Max(max, min);
                    property.vector2Value = new Vector2(min, max);
                }

                EditorGUI.BeginChangeCheck();
                EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, minMaxSlider.Min, minMaxSlider.Max);
                min = EditorGUI.FloatField(minRect, min);
                max = EditorGUI.FloatField(maxRect, max);
                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2Value = new Vector2(min, max);
                }
            }
        }
    }
}
