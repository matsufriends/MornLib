using UnityEditor;
using UnityEngine;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(SpritePreviewAttribute))]
    public sealed class SpritePreviewDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var sprite = property.objectReferenceValue as Sprite;
            var size = ((SpritePreviewAttribute)attribute).Size;
            if (sprite != null)
            {
                var rect = new Rect(position.width - size, position.y + 20, size, size);
                GUI.DrawTexture(rect, sprite.texture, ScaleMode.ScaleToFit);
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var size = ((SpritePreviewAttribute)attribute).Size;
            return EditorGUI.GetPropertyHeight(property, label, true) + size;
        }
    }
}