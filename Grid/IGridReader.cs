namespace MornLib.Grid {
    public interface IGridReader<T> {
        public T this[GridPos gridPos] { get; }
        public GridSize GridSize { get; }
        public bool TrtGet(GridPos gridPos,out T value);
        public bool TrtSet(GridPos gridPos,T value);
    }
}
