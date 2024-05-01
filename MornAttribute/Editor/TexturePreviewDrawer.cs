using UnityEditor;
using UnityEngine;

namespace MornAttribute
{
    [CustomPropertyDrawer(typeof(TexturePreviewAttribute))]
    internal sealed class TexturePreviewDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var texture = property.objectReferenceValue as Texture;
            var size = ((TexturePreviewAttribute)attribute).Size;
            if (texture != null)
            {
                var rect = new Rect(position.width - size, position.y + 20, size, size);
                GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit);
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var size = ((TexturePreviewAttribute)attribute).Size;
            return EditorGUI.GetPropertyHeight(property, label, true) + size;
        }
    }
}