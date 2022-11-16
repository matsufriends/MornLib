namespace MornLib.Grid {
    public interface IGridReader<T> {
        public int Width { get; }
        public int Height { get; }
        public T this[int x,int y] { get; }
        public bool TrtGet(int x,int y,out T value);
        public bool TrtSet(int x,int y,T value);
    }
}
