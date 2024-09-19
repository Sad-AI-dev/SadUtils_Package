using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SadUtils.Types
{
    [Serializable]
    public class UnityDictionary<Key, Value> : IDictionary<Key, Value>, ISerializationCallbackReceiver
    {
        [Serializable]
        public struct Pair
        {
            public Key key;
            public Value value;

            public Pair(Key key, Value value)
            {
                this.key = key;
                this.value = value;
            }

            public Pair(KeyValuePair<Key, Value> pair)
            {
                key = pair.Key;
                value = pair.Value;
            }
        }

        [SerializeField] private List<Pair> dictionary;

        private Dictionary<Key, Value> dict;

        #region Constructors
        // ----------- Constructors -----------
        public UnityDictionary()
        {
            dictionary = new();
            dict = new();
        }

        public UnityDictionary(Dictionary<Key, Value> dictionary)
        {
            this.dictionary = new();
            foreach (KeyValuePair<Key, Value> pair in dictionary)
                this.dictionary.Add(new(pair));

            dict = new(dictionary);
        }
        #endregion

        #region Dictionary Implementation
        // ------------ Dictionary Implementation ------------
        // --- Properties ---
        public int Count => dict.Count;

        public ICollection<Key> Keys => dict.Keys;
        public ICollection<Value> Values => dict.Values;

        public bool IsReadOnly => false;

        public Value this[Key key]
        {
            get => dict[key];
            set => dict[key] = value;
        }

        // --- Methods ---
        public void Add(Key key, Value value) { dict.Add(key, value); }
        public void Add(KeyValuePair<Key, Value> pair) { dict.Add(pair.Key, pair.Value); }

        public bool Remove(Key key) { return dict.Remove(key); }
        public bool Remove(KeyValuePair<Key, Value> pair) { return dict.Remove(pair.Key); }

        public void Clear() { dict.Clear(); }

        public bool Contains(KeyValuePair<Key, Value> pair) { return dict.Contains(pair); }
        public bool ContainsKey(Key key) { return dict.ContainsKey(key); }
        public bool ContainsValue(Value value) { return dict.ContainsValue(value); }

        public bool TryGetValue(Key key, out Value value)
        {
            value = default;

            if (!ContainsKey(key))
                return false;

            value = this[key];
            return true;
        }

        // arrayIndex is the index to start copying from.
        public void CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex)
        {
            array = new KeyValuePair<Key, Value>[Count - arrayIndex];
            int counter = 0;

            foreach (KeyValuePair<Key, Value> pair in dict)
            {
                if (counter >= arrayIndex)
                    array[counter - arrayIndex] = pair;

                counter++;
            }
        }

        public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator() => dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region Serialization
        // ------------ Serialization ------------
        // Dictionary to List
        public void OnBeforeSerialize()
        {
            // Initialize if needed
            dict ??= new();
            dictionary ??= new();

            if (!IsValidList())
                return;

            PopulateList();
        }

        // List to Dictionary
        public void OnAfterDeserialize()
        {
            dict = new();
            PopulateDictionary();
        }

        private void PopulateList()
        {
            dictionary.Clear();

            foreach (KeyValuePair<Key, Value> pair in dict)
                dictionary.Add(new(pair));
        }

        private void PopulateDictionary()
        {
            foreach (Pair listEntry in dictionary)
            {
                if (dict.ContainsKey(listEntry.key))
                    continue;

                dict.Add(listEntry.key, listEntry.value);
            }
        }

        /// <summary>
        /// Scans the dictionary list for duplicate entries
        /// </summary>
        private bool IsValidList()
        {
            HashSet<Key> keys = new();

            foreach (Pair listEntry in dictionary)
            {
                if (keys.Contains(listEntry.key))
                    return false;

                keys.Add(listEntry.key);
            }

            return true;
        }
        #endregion

        #region Explicit Type Casts
        // ----------- Type Casts -----------
        public static implicit operator Dictionary<Key, Value>(UnityDictionary<Key, Value> unityDictionary)
        {
            return unityDictionary.dict;
        }

        public static implicit operator UnityDictionary<Key, Value>(Dictionary<Key, Value> dictionary)
        {
            return new(dictionary);
        }
        #endregion
    }
}
