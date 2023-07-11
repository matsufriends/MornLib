using UnityEngine;

namespace MornDictionary
{
    public abstract class MornDictionaryObjectBase<TKey, TValue> : MornDictionaryBaseInternal<TKey, TValue> where TKey : Object
    {
    }
}
