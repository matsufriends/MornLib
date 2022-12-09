using System.Linq;

namespace MornLib.Cores
{
    public static class MornString
    {
        public static int LongestLengthBySplit(this string message, char split)
        {
            return message.Split('\n').Max(s => s.Length);
        }

        public static int MatchCount(this string message, char match)
        {
            var result = 0;
            foreach (var c in message)
            {
                if (c == match)
                {
                    result++;
                }
            }

            return result;
        }

        public static int MatchCount(this string message, string match)
        {
            var matchIndex = 0;
            var result = 0;
            foreach (var c in message)
            {
                if (c == match[matchIndex])
                {
                    matchIndex++;
                    if (matchIndex == match.Length)
                    {
                        result++;
                        matchIndex = 0;
                    }
                }
                else
                {
                    matchIndex = 0;
                }
            }

            return result;
        }
    }
}
