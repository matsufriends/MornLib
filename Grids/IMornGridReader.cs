namespace MornLib.Grids
{
    public interface IMornGridReader<T>
    {
        public T this[MornGridPos mornGridPos] { get; }
        public MornGridSize Size { get; }
        public bool IsInner(MornGridPos pos);
        public bool TrtGet(MornGridPos mornGridPos, out T value);
    }
}
