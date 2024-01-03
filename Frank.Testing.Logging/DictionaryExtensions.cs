namespace Frank.Testing.Logging;

internal static class DictionaryExtensions
{
    public static TValue GetOrCreate<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, 
        TKey key, 
        Func<TValue> valueFactory)
    {
        if (dictionary.TryGetValue(key, out TValue? value) && value != null)
            return value;

        value = valueFactory();
        dictionary[key] = value;
        return value;
    }
}