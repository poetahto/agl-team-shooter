namespace Application.Gameplay.Dialogue
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A inspector-friendly generator for dictionaries.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <typeparam name="TData">The datatype used to expose these values to the inspector.</typeparam>
    [Serializable]
    public class DictionaryGeneratorAdvanced<TKey, TValue, TData>
        where TData : SerializedKeyValuePair<TKey, TValue>
    {
        [SerializeField]
        private List<TData> dictionary;

        /// <summary>
        /// Creates a new dictionary from the inspector-defined values.
        /// </summary>
        /// <returns>A dictionary filled with inspector-defined values.</returns>
        public Dictionary<TKey, TValue> GenerateDictionary()
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();

            foreach (TData keyValuePair in dictionary)
            {
                result.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return result;
        }
    }
}
