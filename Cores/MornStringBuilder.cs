using System.Text;
using MornLib.Pool;
namespace MornLib.Cores {
    public sealed class MornStringBuilder : IPoolItem {
        private readonly StringBuilder _builder = new();
        private readonly char _split;
        private bool _isFirst;
        public MornStringBuilder(char split) {
            _builder.Clear();
            _split   = split;
            _isFirst = true;
        }
        public void Clear() => _builder.Clear();
        public void Append(string message) {
            if(_isFirst == false) _builder.Append(_split);
            _builder.Append(message);
            _isFirst = false;
        }
        public string Get() => _builder.ToString();
    }
}