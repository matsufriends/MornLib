using UnityEditor;

namespace MornEditor
{
    [CustomEditor(typeof(MornBehaviour), true)]
    [CanEditMultipleObjects]
    internal class MornBehaviourEditor : Editor
    {
        private ButtonDrawer _buttonDrawer;

        private void OnEnable()
        {
            _buttonDrawer = new ButtonDrawer(target);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            _buttonDrawer.DrawButtons(targets);
        }
    }
}