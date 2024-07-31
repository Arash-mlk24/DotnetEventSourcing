namespace DotnetEventSourcing.src.Core.Shared.Types;

public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
{
    public KeyValueList()
    {
    }

    public void Add(TKey key, TValue value)
    {
        Add(KeyValuePair.Create(key, value));
    }

    public void AddRange(Dictionary<TKey, TValue> dic)
    {
        AddRange(dic.Select(x => KeyValuePair.Create(x.Key, x.Value)).ToList());
    }

    public Dictionary<TKey, TValue> AsDictionary()
    => this.ToDictionary((x) => x.Key, x => x.Value);
}