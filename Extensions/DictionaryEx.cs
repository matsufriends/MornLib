using System;
using System.Collections.Generic;
using MornLib.Cores;
namespace MornLib.Extensions {
    public static class DictionaryEx {
        public static void Init<TEnum,T>(ref Dictionary<TEnum,T> dictionary,T value) where TEnum : Enum {
            if(dictionary == null) dictionary = new Dictionary<TEnum,T>();
            foreach(var type in MornEnum<TEnum>.Values()) {
                if(dictionary.ContainsKey(type) == false) dictionary.Add(type,value);
            }
        }
    }
}
