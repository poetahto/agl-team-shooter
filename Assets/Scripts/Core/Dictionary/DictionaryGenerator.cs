namespace Application.Gameplay.Dialogue
{
    using System;

    /// <summary>
    /// A inspector-friendly generator for dictionaries.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    [Serializable]
    public class DictionaryGenerator<TKey, TValue> :
        DictionaryGeneratorAdvanced<TKey, TValue, SerializeFieldPair<TKey, TValue>>
    {
    }
}
