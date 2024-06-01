using System;
using MornAttribute;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornUI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class MornUIMonoBase : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected RectTransform RectTransform;
        [Header("Navigation")]
        [SerializeField] private MornUIMonoBase up;
        [SerializeField] private MornUIMonoBase down;
        [SerializeField] private MornUIMonoBase left;
        [SerializeField] private MornUIMonoBase right;
        [SerializeField] private MornUIMonoBase cancel;
        private MornUIControllerMono cachedParent;
        public MornUIControllerMono Parent => cachedParent ??= GetComponentInParent<MornUIControllerMono>();

        internal void AddSurround(Action<MornUIMonoBase> addSurround)
        {
            if (up != null)
            {
                addSurround(up);
            }

            if (down != null)
            {
                addSurround(down);
            }

            if (left != null)
            {
                addSurround(left);
            }

            if (right != null)
            {
                addSurround(right);
            }

            if (cancel != null)
            {
                addSurround(cancel);
            }
        }

        public virtual void OnSubmit()
        {
        }

        public virtual void OnCancel(out MornUIMonoBase nextFocus)
        {
            nextFocus = cancel;
        }

        public virtual void OnMove(Vector2 input, out MornUIMonoBase nextFocus)
        {
            nextFocus = input.ToDir() switch
            {
                    MornUIDirType.None  => null,
                    MornUIDirType.Up    => up,
                    MornUIDirType.Down  => down,
                    MornUIDirType.Left  => left,
                    MornUIDirType.Right => right,
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
                    _                   => position,
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
                    _                   => position,
            };
        }

        private void OnDrawGizmos()
        {
            var gizmosColor = Gizmos.color;
            if (up != null)
            {
                Gizmos.color = Color.red;
                DrawGizmoArrow(GetFromPos(MornUIDirType.Up), up.GetToPos(MornUIDirType.Up));
            }

            if (down != null)
            {
                Gizmos.color = Color.blue;
                DrawGizmoArrow(GetFromPos(MornUIDirType.Down), down.GetToPos(MornUIDirType.Down));
            }

            if (left != null)
            {
                Gizmos.color = Color.yellow;
                DrawGizmoArrow(GetFromPos(MornUIDirType.Left), left.GetToPos(MornUIDirType.Left));
            }

            if (right != null)
            {
                Gizmos.color = Color.green;
                DrawGizmoArrow(GetFromPos(MornUIDirType.Right), right.GetToPos(MornUIDirType.Right));
            }

            if (cancel != null)
            {
                Gizmos.color = Color.magenta;
                DrawGizmoArrow(GetFromPos(MornUIDirType.None), cancel.GetToPos(MornUIDirType.None));
            }

            Gizmos.color = gizmosColor;
        }

        private static void DrawGizmoArrow(Vector3 from, Vector3 to, float arrowHeadLength = 5f, float arrowHeadAngle = 30.0f)
        {
            if (from == to)
                return;
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