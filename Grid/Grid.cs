namespace MornLib.Grid {
    public class Grid<T> : IGridReader<T> {
        private readonly T[,] _board;
        public int Width { get; }
        public int Height { get; }
        public T this[int x,int y] {
            get => _board[x,y];
            set => _board[x,y] = value;
        }
        public Grid(int width,int height) {
            Width  = width;
            Height = height;
            _board = new T[Width,Height];
        }
        public bool TrtGet(int x,int y,out T value) {
            var isValid = 0 <= x && x < Width && 0 <= y && y < Height;
            value = isValid ? _board[x,y] : default;
            return isValid;
        }
        public bool TrtSet(int x,int y,T value) {
            var isValid = 0 <= x && x < Width && 0 <= y && y < Height;
            if(isValid) _board[x,y] = value;
            return isValid;
        }
    }
}
