using System;

namespace MornLib.Types
{
    public sealed class MornTypeSystem
    {
        public string Stack { get; private set; }

        public bool TryAppendChar(char addChar, out string hiragana)
        {
            Stack += addChar;
            hiragana = UpdateHiraganaList(out var isKeep);
            if (hiragana != "")
            {
                Stack = isKeep ? Stack[^1].ToString() : "";
            }

            return hiragana != "";
        }

        private string UpdateHiraganaList(out bool isKeep)
        {
            isKeep = true;
            //っ
            if (TryConvert(Convert2ToXtu, 2, out var hiragana))
            {
                return hiragana;
            }

            //ん
            if (TryConvert(Convert2ToNn, 2, out hiragana))
            {
                return hiragana;
            }

            isKeep = false;
            //4文字
            if (TryConvert(Convert4ToHiragana, 4, out hiragana))
            {
                return hiragana;
            }

            //3文字
            if (TryConvert(Convert3ToHiragana, 3, out hiragana))
            {
                return hiragana;
            }

            //2文字
            if (TryConvert(Convert2ToHiragana, 2, out hiragana))
            {
                return hiragana;
            }

            //1文字
            if (TryConvert(Convert1ToHiragana, 1, out hiragana))
            {
                return hiragana;
            }

            return "";
        }

        private bool TryConvert(Func<string, string> func, int count, out string hiragana)
        {
            if (Stack.Length < count)
            {
                hiragana = "";
                return false;
            }

            hiragana = func(Stack[^count ..]);
            return hiragana != "";
        }

        private static string Convert4ToHiragana(string check)
        {
            switch (check)
            {
                case "ltsu": return "っ";
                case "xtsu": return "っ";
            }

            return "";
        }

        private static string Convert3ToHiragana(string check)
        {
            switch (check)
            {
                case "wyi": return "ゐ";
                case "kya": return "きゃ";
                case "kyi": return "きぃ";
                case "kyu": return "きゅ";
                case "kye": return "きぇ";
                case "kyo": return "きょ";
                case "sya": return "しゃ";
                case "syi": return "しぃ";
                case "syu": return "しゅ";
                case "sye": return "しぇ";
                case "syo": return "しょ";
                case "sha": return "しゃ";
                case "shi": return "し";
                case "shu": return "しゅ";
                case "she": return "しぇ";
                case "sho": return "しょ";
                case "swa": return "すぁ";
                case "swi": return "すぃ";
                case "swu": return "すぅ";
                case "swe": return "すぇ";
                case "swo": return "すぉ";
                case "tsu": return "つ";
                case "tya": return "ちゃ";
                case "tyi": return "ちぃ";
                case "tyu": return "ちゅ";
                case "tye": return "ちぇ";
                case "tyo": return "ちょ";
                case "cha": return "ちゃ";
                case "chi": return "ち";
                case "chu": return "ちゅ";
                case "che": return "ちぇ";
                case "cho": return "ちょ";
                case "tha": return "てゃ";
                case "thi": return "てぃ";
                case "thu": return "てゅ";
                case "the": return "てぇ";
                case "tho": return "てょ";
                case "twa": return "とぁ";
                case "twi": return "とぃ";
                case "twu": return "とぅ";
                case "twe": return "とぇ";
                case "two": return "とぉ";
                case "dha": return "でゃ";
                case "dhi": return "でぃ";
                case "dhu": return "でゅ";
                case "dhe": return "でぇ";
                case "dho": return "でょ";
                case "dwa": return "どぁ";
                case "dwi": return "どぃ";
                case "dwu": return "どぅ";
                case "dwe": return "どぇ";
                case "dwo": return "どぉ";
                case "nya": return "にゃ";
                case "nyi": return "にぃ";
                case "nyu": return "にゅ";
                case "nye": return "にぇ";
                case "nyo": return "にょ";
                case "hya": return "ひゃ";
                case "hyi": return "ひぃ";
                case "hyu": return "ひゅ";
                case "hye": return "ひぇ";
                case "hyo": return "ひょ";
                case "mya": return "みゃ";
                case "myi": return "みぃ";
                case "myu": return "みゅ";
                case "mye": return "みぇ";
                case "myo": return "みょ";
                case "rya": return "りゃ";
                case "ryi": return "りぃ";
                case "ryu": return "りゅ";
                case "rye": return "りぇ";
                case "ryo": return "りょ";
                case "gya": return "ぎゃ";
                case "gyi": return "ぎぃ";
                case "gyu": return "ぎゅ";
                case "gye": return "ぎぇ";
                case "gyo": return "ぎょ";
                case "gwa": return "ぐぁ";
                case "gwi": return "ぐぃ";
                case "gwu": return "ぐぅ";
                case "gwe": return "ぐぇ";
                case "gwo": return "ぐぉ";
                case "jya": return "じゃ";
                case "jyi": return "じぃ";
                case "jyu": return "じゅ";
                case "jye": return "じぇ";
                case "jyo": return "じょ";
                case "zya": return "じゃ";
                case "zyi": return "じぃ";
                case "zyu": return "じゅ";
                case "zye": return "じぇ";
                case "zyo": return "じょ";
                case "dya": return "ぢゃ";
                case "dyi": return "ぢぃ";
                case "dyu": return "ぢゅ";
                case "dye": return "ぢぇ";
                case "dyo": return "ぢょ";
                case "bya": return "びゃ";
                case "byi": return "びぃ";
                case "byu": return "びゅ";
                case "bye": return "びぇ";
                case "byo": return "びょ";
                case "pya": return "ぴゃ";
                case "pyi": return "ぴぃ";
                case "pyu": return "ぴゅ";
                case "pye": return "ぴぇ";
                case "pyo": return "ぴょ";
                case "lya": return "ゃ";
                case "lyi": return "ぃ";
                case "lyu": return "ゅ";
                case "lye": return "ぇ";
                case "lyo": return "ょ";
                case "xya": return "ゃ";
                case "xyi": return "ぃ";
                case "xyu": return "ゅ";
                case "xye": return "ぇ";
                case "xyo": return "ょ";
                case "ltu": return "っ";
                case "xtu": return "っ";
                case "wha": return "うぁ";
                case "who": return "うぉ";
            }

            return "";
        }

        private static string Convert2ToXtu(string check)
        {
            switch (check)
            {
                case "qq":
                case "rr":
                case "tt":
                case "yy":
                case "pp":
                case "ss":
                case "dd":
                case "ff":
                case "gg":
                case "hh":
                case "jj":
                case "kk":
                case "ll":
                case "zz":
                case "xx":
                case "cc":
                case "vv":
                case "bb":
                case "mm": return "っ";
            }

            return "";
        }

        private static string Convert2ToNn(string check)
        {
            switch (check)
            {
                case "nq":
                case "nw":
                case "nr":
                case "nt":
                case "np":
                case "ns":
                case "nd":
                case "nf":
                case "ng":
                case "nh":
                case "nj":
                case "nk":
                case "nl":
                case "nz":
                case "nx":
                case "nc":
                case "nv":
                case "nb":
                case "nm": return "ん";
            }

            return "";
        }

        private static string Convert2ToHiragana(string check)
        {
            switch (check)
            {
                case "ka": return "か";
                case "ki": return "き";
                case "ku": return "く";
                case "ke": return "け";
                case "ko": return "こ";
                case "sa": return "さ";
                case "si": return "し";
                case "su": return "す";
                case "se": return "せ";
                case "so": return "そ";
                case "ta": return "た";
                case "ti": return "ち";
                case "tu": return "つ";
                case "te": return "て";
                case "to": return "と";
                case "na": return "な";
                case "ni": return "に";
                case "nu": return "ぬ";
                case "ne": return "ね";
                case "no": return "の";
                case "ha": return "は";
                case "hi": return "ひ";
                case "hu": return "ふ";
                case "he": return "へ";
                case "ho": return "ほ";
                case "ma": return "ま";
                case "mi": return "み";
                case "mu": return "む";
                case "me": return "め";
                case "mo": return "も";
                case "ya": return "や";
                case "yu": return "ゆ";
                case "ye": return "いぇ";
                case "yo": return "よ";
                case "ra": return "ら";
                case "ri": return "り";
                case "ru": return "る";
                case "re": return "れ";
                case "ro": return "ろ";
                case "wa": return "わ";
                case "wi": return "うぃ";
                case "wu": return "う";
                case "we": return "うぇ";
                case "wo": return "を";
                case "nn": return "ん";
                case "xn": return "ん";
                case "vu": return "ぶ";
                case "qa": return "くぁ";
                case "qi": return "くぃ";
                case "qu": return "く";
                case "qe": return "くぇ";
                case "qo": return "くぉ";
                case "fa": return "ふぁ";
                case "fi": return "ふぃ";
                case "fu": return "ふ";
                case "fe": return "ふぇ";
                case "fo": return "ふぉ";
                case "ga": return "が";
                case "gi": return "ぎ";
                case "gu": return "ぐ";
                case "ge": return "げ";
                case "go": return "ご";
                case "za": return "ざ";
                case "zi": return "じ";
                case "zu": return "ず";
                case "ze": return "ぜ";
                case "zo": return "ぞ";
                case "ja": return "じゃ";
                case "ji": return "じ";
                case "ju": return "じゅ";
                case "je": return "じぇ";
                case "jo": return "じょ";
                case "da": return "だ";
                case "di": return "ぢ";
                case "du": return "づ";
                case "de": return "で";
                case "do": return "ど";
                case "ba": return "ば";
                case "bi": return "び";
                case "bu": return "ぶ";
                case "be": return "べ";
                case "bo": return "ぼ";
                case "pa": return "ぱ";
                case "pi": return "ぴ";
                case "pu": return "ぷ";
                case "pe": return "ぺ";
                case "po": return "ぽ";
                case "la": return "ぁ";
                case "li": return "ぃ";
                case "lu": return "ぅ";
                case "le": return "ぇ";
                case "lo": return "ぉ";
                case "xa": return "ぁ";
                case "xi": return "ぃ";
                case "xu": return "ぅ";
                case "xe": return "ぇ";
                case "xo": return "ぉ";
            }

            return "";
        }

        private static string Convert1ToHiragana(string check)
        {
            switch (check)
            {
                case "a": return "あ";
                case "i": return "い";
                case "u": return "う";
                case "e": return "え";
                case "o": return "お";
                case "ー": return "ー";
                case "・": return "・";
            }

            return "";
        }
    }
}