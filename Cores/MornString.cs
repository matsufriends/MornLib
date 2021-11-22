using System.Text;
namespace MornLib.Cores {
    public static class MornString {
        private static readonly StringBuilder s_builder = new StringBuilder();
        private static          char          s_split;
        private static          bool          s_isFirst;
        public static void Init(char split) {
            s_builder.Clear();
            s_split   = split;
            s_isFirst = true;
        }
        public static void Append(string message) {
            if (s_isFirst == false) s_builder.Append(s_split);
            s_builder.Append(message);
            s_isFirst = false;
        }
        public static string Get() {
            return s_builder.ToString();
        }
    }
}