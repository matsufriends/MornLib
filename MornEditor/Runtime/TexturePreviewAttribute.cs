using UnityEngine;

namespace MornEditor
{
    public sealed class TexturePreviewAttribute : PropertyAttribute
    {
        public readonly float Size;

        public TexturePreviewAttribute(float size = 200)
        {
            Size = size;
        }
    }
}