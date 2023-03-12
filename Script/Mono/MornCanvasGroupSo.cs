using MornLib.Singletons;
using UnityEngine;

namespace MornLib.Mono
{
    [CreateAssetMenu(fileName = nameof(MornCanvasGroupSo), menuName = nameof(MornCanvasGroupSo))]
    public class MornCanvasGroupSo : MornSingletonSo<MornCanvasGroupSo>
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        public int Width => _width;
        public int Height => _height;
    }
}
