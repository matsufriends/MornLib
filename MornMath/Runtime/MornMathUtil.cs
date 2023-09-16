namespace MornMath
{
    public static class MornMathUtil
    {
        public static bool BitEqual(this int self, int flag)
        {
            return (self & flag) == flag;
        }

        public static int BitAdd(this int self, int flag)
        {
            return self | flag;
        }

        public static int BitRemove(this int self, int flag)
        {
            return self & ~flag;
        }

        public static int BitXor(this int self, int flag)
        {
            return self ^ flag;
        }
    }
}