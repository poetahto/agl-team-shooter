using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Poetools.UI.Serialization
{
    /// <summary>
    /// A default serializer implementation that uses PlayerPrefs.
    /// </summary>
    public class PlayerPrefsSerializer : ISerializer
    {
        /// <summary>
        /// Gets the global instance of <see cref="PlayerPrefsSerializer"/>.
        /// </summary>
        public static PlayerPrefsSerializer Instance { get; } = new PlayerPrefsSerializer();

        // Disallow people from creating new instances of this class - since PlayerPrefs is all global
        // state anyways, creating new instances doesn't really make sense. Hence, the singleton
        // "Instance" getter is the only way to access this class.
        private PlayerPrefsSerializer()
        {
        }

        /// <inheritdoc />
        public void Write<T>(string id, T data)
        {
            switch (data)
            {
                case string stringValue:
                    PlayerPrefs.SetString(id, stringValue);
                    break;

                case int intValue:
                    PlayerPrefs.SetInt(id, intValue);
                    break;

                case float floatValue:
                    PlayerPrefs.SetFloat(id, floatValue);
                    break;

                case bool boolValue:
                    PlayerPrefs.SetInt(id, boolValue ? 1 : 0);
                    break;
            }
        }

        /// <inheritdoc />
        public T Read<T>(string id)
        {
            var type = typeof(T);

            if (type == typeof(string))
            {
                string stringValue = PlayerPrefs.GetString(id);
                return UnsafeUtility.As<string, T>(ref stringValue);
            }

            if (type == typeof(int))
            {
                int intValue = PlayerPrefs.GetInt(id);
                return UnsafeUtility.As<int, T>(ref intValue);
            }

            if (type == typeof(float))
            {
                float floatValue = PlayerPrefs.GetFloat(id);
                return UnsafeUtility.As<float, T>(ref floatValue);
            }

            if (type == typeof(bool))
            {
                bool boolValue = PlayerPrefs.GetInt(id) != 0;
                return UnsafeUtility.As<bool, T>(ref boolValue);
            }

            throw new Exception("Invalid Type, PlayerPrefs can only handle string, int, float, and bool.");
        }
    }
}
