namespace MornLib.Grid {
    public readonly struct GridSize {
        public int Width { get; }
        public int Height { get; }

        public GridSize(int width,int height) {
            Width  = width;
            Height = height;
        }
    }
}
