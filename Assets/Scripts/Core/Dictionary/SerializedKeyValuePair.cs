namespace Application.Gameplay.Dialogue
{
    /// <summary>
    /// A key-value-pair that can be displayed in the Unity editor.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public abstract class SerializedKeyValuePair<TKey, TValue>
    {
        /// <summary>
        /// Gets the key. Used for indexing and looking up the value..
        /// </summary>
        public abstract TKey Key { get; }

        /// <summary>
        /// Gets the value. Can be any useful data that is stored and accessed.
        /// </summary>
        public abstract TValue Value { get; }
    }
}
