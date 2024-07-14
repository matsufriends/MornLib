using System;

namespace MornDictionary
{
    public abstract class MornDictionaryEnumBase<TKey, TValue> : MornDictionaryBaseInternal<TKey, TValue>
        where TKey : Enum
    {
    }
}