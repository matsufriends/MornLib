using System.Collections.Generic;

namespace MornLib.Grids
{
    public abstract class MornGrid<T> : IMornGridReader<T>
    {
        private readonly T[,] _board;

        public T this[MornGridPos pos]
        {
            get => _board[pos.X, pos.Y];
            set => _board[pos.X, pos.Y] = value;
        }

        public MornGridSize Size { get; }

        protected MornGrid(MornGridSize size)
        {
            Size = size;
            _board = new T[size.Width, size.Height];
        }

        public bool IsInner(MornGridPos pos)
        {
            return 0 <= pos.X && pos.X < Size.Width && 0 <= pos.Y && pos.Y < Size.Height;
        }

        public bool TrtGet(MornGridPos pos, out T value)
        {
            var isValid = IsInner(pos);
            value = isValid ? _board[pos.X, pos.Y] : default;
            return isValid;
        }

        public bool TrtSet(MornGridPos pos, T value)
        {
            var isValid = 0 <= pos.X && pos.X < Size.Width && 0 <= pos.Y && pos.Y < Size.Height &&
                          !EqualityComparer<T>.Default.Equals(_board[pos.X, pos.Y], value);
            if (isValid)
            {
                _board[pos.X, pos.Y] = value;
            }

            return isValid;
        }
    }
}
