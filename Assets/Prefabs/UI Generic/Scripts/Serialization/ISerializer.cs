using JetBrains.Annotations;

namespace Poetools.UI.Serialization
{
    /// <summary>
    /// An object that can store arbitrary data, keyed by a string.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Writes some arbitrary data into storage, associated with a unique key.
        /// </summary>
        /// <param name="id">The key that indexes this data.</param>
        /// <param name="data">The data that should be stored.</param>
        /// <typeparam name="T">The type of data to be stored.</typeparam>
        void Write<T>(string id, T data);

        /// <summary>
        /// Retrieves some arbitrary data that is associated with a unqieu key.
        /// </summary>
        /// <param name="id">The unique key for the data to retrieve.</param>
        /// <typeparam name="T">The type of data to retrieve.</typeparam>
        /// <returns>The stored data, if any.</returns>
        [CanBeNull]
        T Read<T>(string id);
    }
}
