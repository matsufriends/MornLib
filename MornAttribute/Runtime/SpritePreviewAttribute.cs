using UnityEngine;

namespace MornAttribute
{
    public sealed class SpritePreviewAttribute : PropertyAttribute
    {
        public readonly float Size;

        public SpritePreviewAttribute(float size = 200)
        {
            Size = size;
        }
    }
}