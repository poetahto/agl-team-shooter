namespace Application.Gameplay.Dialogue
{
    using System;
    using UnityEngine;

    /// <summary>
    /// A more advanced implementation of a serialized key-value-pair. The key uses [SerializeField],
    /// while the value uses [SerializeReference].
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    [Serializable]
    public sealed class SerializeReferencePair<TKey, TValue> : SerializedKeyValuePair<TKey, TValue>
    {
        [SerializeField]
        private TKey key;

        [SerializeReference]
        private TValue value;

        /// <inheritdoc/>
        public override TKey Key => key;

        /// <inheritdoc/>
        public override TValue Value => value;
    }
}
