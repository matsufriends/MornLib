using System.Collections.Generic;
namespace MornLib.Grid {
    public class Grid<T> : IGridReader<T> {
        private readonly T[,] _board;
        public T this[GridPos pos] {
            get => _board[pos.X,pos.Y];
            set => _board[pos.X,pos.Y] = value;
        }
        public GridSize GridSize { get; }
        public Grid(GridSize gridSize) {
            GridSize = gridSize;
            _board   = new T[gridSize.Width,gridSize.Height];
        }
        public bool TrtGet(GridPos pos,out T value) {
            var isValid = 0 <= pos.X && pos.X < GridSize.Width && 0 <= pos.Y && pos.Y < GridSize.Height;
            value = isValid ? _board[pos.X,pos.Y] : default;
            return isValid;
        }
        public bool TrtSet(GridPos pos,T value) {
            var isValid = 0 <= pos.X
                       && pos.X < GridSize.Width
                       && 0 <= pos.Y
                       && pos.Y < GridSize.Height
                       && !EqualityComparer<T>.Default.Equals(_board[pos.X,pos.Y],value);
            if(isValid) _board[pos.X,pos.Y] = value;
            return isValid;
        }
    }
}
