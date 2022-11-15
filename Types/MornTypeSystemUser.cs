namespace MornLib.Types {
    public sealed class MornTypeSystemUser {
        public string Typed { get; private set; }
        public string NotTyped { get; private set; }
        public MornTypeSystemUser(string typed,string notTyped) {
            Typed    = typed;
            NotTyped = notTyped;
        }
        public bool TryAppend(string hiragana) {
            if(NotTyped.Length < hiragana.Length) return false;
            var next = NotTyped[..hiragana.Length];
            if(hiragana == "い" && next == "ゐ") hiragana = "ゐ";
            if(hiragana != next) return false;
            Typed    += hiragana;
            NotTyped =  NotTyped[hiragana.Length..];
            while(NotTyped.Length > 0 && NotTyped[0] == ' ') {
                Typed    += " ";
                NotTyped =  NotTyped[1..];
            }
            return true;
        }
    }
}
