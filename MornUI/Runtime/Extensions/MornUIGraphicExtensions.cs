using UnityEngine.UI;

namespace MornUI
{
    public static class MornUIGraphicExtensions
    {
        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            var color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }
}
