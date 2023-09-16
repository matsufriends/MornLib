using System.Collections.Generic;
using MornEnum;
using UnityEditor;
using UnityEngine;

namespace MornBeat
{
    //BeatActionSoのEditor拡張
    [CustomEditor(typeof(MornBeatActionSettingSoBase<>), true)]
    internal sealed class MornBeatActionSettingSoEditor : Editor
    {
        private SerializedProperty _measureTick;
        private SerializedProperty _beatAction;
        private Color _cachedBackgroundColor;
        private readonly HashSet<int> _tickHashSet = new();
        private const int InputWidth = 30;
        private const int ButtonWidth = 50;

        private void OnEnable()
        {
            _measureTick = serializedObject.FindProperty("_measureTick");
            _beatAction = serializedObject.FindProperty("_beatAction");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            _tickHashSet.Clear();
            _cachedBackgroundColor = GUI.backgroundColor;
            var cachedMeasure = 0;
            for (var i = 0; i < _beatAction.arraySize; i++)
            {
                var row = _beatAction.GetArrayElementAtIndex(i);
                var measureProperty = row.FindPropertyRelative("_measure");
                var tickProperty = row.FindPropertyRelative("_tick");
                var beatActionTypeProperty = row.FindPropertyRelative("_beatActionType");
                if (measureProperty.intValue != cachedMeasure)
                {
                    GUILayout.Space(10);
                    cachedMeasure = measureProperty.intValue;
                }

                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
                    ShowMeasureAndTick(measureProperty, tickProperty);
                    ShowBeatActionType(beatActionTypeProperty);
                    if (GUILayout.Button("Delete", GUILayout.Width(ButtonWidth)))
                    {
                        _beatAction.DeleteArrayElementAtIndex(i);
                        break;
                    }

                    if (GUILayout.Button("Add", GUILayout.Width(ButtonWidth)))
                    {
                        _beatAction.InsertArrayElementAtIndex(i);
                        break;
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowMeasureAndTick(SerializedProperty measureProperty, SerializedProperty tickProperty)
        {
            var tick = _measureTick.intValue * measureProperty.intValue + tickProperty.intValue;
            GUI.backgroundColor = _tickHashSet.Add(tick) ? _cachedBackgroundColor : Color.red;
            EditorGUILayout.PropertyField(measureProperty, GUIContent.none, GUILayout.Width(InputWidth));
            EditorGUILayout.PropertyField(tickProperty, GUIContent.none, GUILayout.Width(InputWidth));
            GUI.backgroundColor = _cachedBackgroundColor;
        }

        private void ShowBeatActionType(SerializedProperty beatActionType)
        {
            var tuples = ((IMornBeatActionSettingSo)target).DisplayTuples;
            var width = (EditorGUIUtility.currentViewWidth - InputWidth * 2 - ButtonWidth * 2 - 50) / tuples.Length;
            var flag = beatActionType.enumValueFlag;
            foreach (var tuple in tuples)
            {
                var containFlag = MornEnumUtil.Equal(flag, (int)(object)tuple.Item1);
                GUI.backgroundColor = containFlag ? tuple.Item3 : _cachedBackgroundColor;
                if (GUILayout.Button(tuple.Item2, GUILayout.Width(width)))
                {
                    if (containFlag)
                    {
                        beatActionType.enumValueFlag = MornEnumUtil.Remove(flag, (int)(object)tuple.Item1);
                    }
                    else
                    {
                        beatActionType.enumValueFlag = MornEnumUtil.Add(flag, (int)(object)tuple.Item1);
                    }
                }

                GUI.backgroundColor = _cachedBackgroundColor;
            }
        }
    }
}