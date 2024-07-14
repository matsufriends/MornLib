using System;

namespace MornLib.Cores
{
    public static class MornTimeSpan
    {
        public static TimeSpan ToTimeSpanAsSeconds(this float seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }
    }
}