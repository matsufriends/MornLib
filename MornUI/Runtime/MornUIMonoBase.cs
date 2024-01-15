using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornUI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class MornUIMonoBase : MonoBehaviour
    {
        [SerializeField] protected RectTransform RectTransform;
        [SerializeField] private MornUIMonoBase _up;
        [SerializeField] private MornUIMonoBase _down;
        [SerializeField] private MornUIMonoBase _left;
        [SerializeField] private MornUIMonoBase _right;
        [SerializeField] private MornUIMonoBase _cancel;

        internal void AddSurround(Action<MornUIMonoBase> addSurround)
        {
            if (_up != null)
            {
                addSurround(_up);
            }

            if (_down != null)
            {
                addSurround(_down);
            }

            if (_left != null)
            {
                addSurround(_left);
            }

            if (_right != null)
            {
                addSurround(_right);
            }

            if (_cancel != null)
            {
                addSurround(_cancel);
            }
        }

        public virtual void OnSubmit()
        {
        }

        public virtual void OnCancel(out MornUIMonoBase nextFocus)
        {
            nextFocus = _cancel;
        }

        public virtual void OnMove(Vector2 input, out MornUIMonoBase nextFocus)
        {
            nextFocus = input.ToDir() switch
            {
                    MornUIDirType.None  => null,
                    MornUIDirType.Up    => _up,
                    MornUIDirType.Down  => _down,
                    MornUIDirType.Left  => _left,
                    MornUIDirType.Right => _right,
                    _                   => throw new ArgumentOutOfRangeException(),
            };
        }

        public abstract void OnFocus(bool isInitial);
        public abstract void OnUnFocus(bool isInitial);

        protected virtual void Reset()
        {
            RectTransform = GetComponent<RectTransform>();
        }

#if UNITY_EDITOR
        private Vector2 GetFromPos(MornUIDirType dir)
        {
            Vector2 position = RectTransform.position;
            var sizeDelta = RectTransform.sizeDelta;
            var lossyScale = RectTransform.lossyScale;
            return dir switch
            {
                    MornUIDirType.Up    => position + new Vector2(sizeDelta.x / 4f * lossyScale.x, sizeDelta.y / 2 * lossyScale.y),
                    MornUIDirType.Down  => position + new Vector2(-sizeDelta.x / 4f * lossyScale.x, -sizeDelta.y / 2 * lossyScale.y),
                    MornUIDirType.Left  => position + new Vector2(-sizeDelta.x / 2 * lossyScale.x, sizeDelta.y / 4f * lossyScale.y),
                    MornUIDirType.Right => position + new Vector2(sizeDelta.x / 2 * lossyScale.x, -sizeDelta.y / 4f * lossyScale.y),
                    _                   => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
            };
        }

        private Vector2 GetToPos(MornUIDirType dir)
        {
            Vector2 position = RectTransform.position;
            var sizeDelta = RectTransform.sizeDelta;
            var lossyScale = RectTransform.lossyScale;
            return dir switch
            {
                    MornUIDirType.Up    => position + new Vector2(sizeDelta.x / 4f * lossyScale.x, -sizeDelta.y / 2 * lossyScale.y),
                    MornUIDirType.Down  => position + new Vector2(-sizeDelta.x / 4f * lossyScale.x, sizeDelta.y / 2 * lossyScale.y),
                    MornUIDirType.Left  => position + new Vector2(sizeDelta.x / 2 * lossyScale.x, sizeDelta.y / 4f * lossyScale.y),
                    MornUIDirType.Right => position + new Vector2(-sizeDelta.x / 2 * lossyScale.x, -sizeDelta.y / 4f * lossyScale.y),
                    _                   => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
            };
        }

        private void OnDrawGizmos()
        {
            var gizmosColor = Gizmos.color;
            if (_up != null)
            {
                Gizmos.color = Color.red;
                DrawGizmoArrow(GetFromPos(MornUIDirType.Up), _up.GetToPos(MornUIDirType.Up));
            }

            if (_down != null)
            {
                Gizmos.color = Color.blue;
                DrawGizmoArrow(GetFromPos(MornUIDirType.Down), _down.GetToPos(MornUIDirType.Down));
            }

            if (_left != null)
            {
                Gizmos.color = Color.yellow;
                DrawGizmoArrow(GetFromPos(MornUIDirType.Left), _left.GetToPos(MornUIDirType.Left));
            }

            if (_right != null)
            {
                Gizmos.color = Color.green;
                DrawGizmoArrow(GetFromPos(MornUIDirType.Right), _right.GetToPos(MornUIDirType.Right));
            }

            Gizmos.color = gizmosColor;
        }

        private static void DrawGizmoArrow(Vector3 from, Vector3 to, float arrowHeadLength = 5f, float arrowHeadAngle = 30.0f)
        {
            Gizmos.DrawLine(from, to);
            var direction = to - from;
            var right = Quaternion.LookRotation(direction, Vector3.forward) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
            var left = Quaternion.LookRotation(direction, Vector3.forward) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
            Gizmos.DrawLine(to, to + right * arrowHeadLength);
            Gizmos.DrawLine(to, to + left * arrowHeadLength);
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MornUIMonoBase), true)]
    public sealed class MornUIMonoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var ui = (MornUIMonoBase)target;
            if (GUILayout.Button("Focus"))
            {
                ui.OnFocus(false);
                EditorUtility.SetDirty(ui);
            }

            if (GUILayout.Button("UnFocus"))
            {
                ui.OnUnFocus(false);
                EditorUtility.SetDirty(ui);
            }
        }
    }
#endif
}