namespace MornBeat
{
    internal static class MornBeatUtil
    {
        internal static double InverseLerp(double a, double b, double value)
        {
            var dif = b - a;
            return (value - a) / dif;
        }

        internal static double Lerp(double a, double b, double t)
        {
            return a + (b - a) * t;
        }
    }
}