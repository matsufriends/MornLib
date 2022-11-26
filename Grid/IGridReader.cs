namespace MornLib.Grid
{
    public interface IGridReader<T>
    {
        public T this[GridPos gridPos] { get; }
        public GridSize Size { get; }
        public bool IsInner(GridPos pos);
        public bool TrtGet(GridPos gridPos, out T value);
    }
}