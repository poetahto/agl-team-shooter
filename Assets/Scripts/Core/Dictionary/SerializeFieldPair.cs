namespace Application.Gameplay.Dialogue
{
    using System;
    using UnityEngine;

    /// <summary>
    /// A typical implementation of a serialized key-value-pair. Each field is serialized with
    /// the [SerializeField] attribute. In most cases, this is fine.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    [Serializable]
    public sealed class SerializeFieldPair<TKey, TValue> : SerializedKeyValuePair<TKey, TValue>
    {
        [SerializeField]
        private TKey key;

        [SerializeField]
        private TValue value;

        /// <inheritdoc/>
        public override TKey Key => key;

        /// <inheritdoc/>
        public override TValue Value => value;
    }
}
