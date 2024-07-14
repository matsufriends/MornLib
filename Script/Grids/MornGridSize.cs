namespace MornLib.Grids
{
    public readonly struct MornGridSize
    {
        public int Width { get; }
        public int Height { get; }

        public MornGridSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}