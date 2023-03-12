namespace MornLib.Grids
{
    public readonly struct MornGridPos
    {
        public int X { get; }
        public int Y { get; }

        public MornGridPos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static MornGridPos operator +(MornGridPos a, MornGridPos b)
        {
            return new MornGridPos(a.X + b.X, a.Y + b.Y);
        }

        public static MornGridPos operator -(MornGridPos a, MornGridPos b)
        {
            return new MornGridPos(a.X - b.X, a.Y - b.Y);
        }

        public static MornGridPos operator *(MornGridPos a, int b)
        {
            return new MornGridPos(a.X * b, a.Y * b);
        }

        public static MornGridPos operator /(MornGridPos a, int b)
        {
            return new MornGridPos(a.X / b, a.Y / b);
        }
    }
}
