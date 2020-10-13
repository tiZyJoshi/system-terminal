using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Views.Collections
{
    public sealed class ViewDictionary<T> : IDictionary<string, T>
        where T : class
    {
        public int Count => _dictionary.Count;

        public ICollection<string> Keys => _dictionary.Keys;

        public ICollection<T> Values => _dictionary.Values;

        bool ICollection<KeyValuePair<string, T>>.IsReadOnly => false;

        public T this[string key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value ?? throw new ArgumentNullException(nameof(value));
        }

        readonly Dictionary<string, T> _dictionary = new();

        public void Add(string key, T value)
        {
            _dictionary.Add(key, value ?? throw new ArgumentNullException(nameof(value)));
        }

        public bool TryAdd(string key, T value)
        {
            return _dictionary.TryAdd(key, value ?? throw new ArgumentNullException(nameof(value)));
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        public bool Remove(string key, [MaybeNullWhen(false)] out T value)
        {
            return _dictionary.Remove(key, out value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool ContainsValue(T value)
        {
            return _dictionary.ContainsValue(value ?? throw new ArgumentNullException(nameof(value)));
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out T value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item)
        {
            _ = item.Value ?? throw new ArgumentNullException("value");

            ((ICollection<KeyValuePair<string, T>>)_dictionary).Add(item);
        }

        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
        {
            _ = item.Value ?? throw new ArgumentNullException("value");

            return ((ICollection<KeyValuePair<string, T>>)_dictionary).Remove(item);
        }

        bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item)
        {
            _ = item.Value ?? throw new ArgumentNullException("value");

            return ((ICollection<KeyValuePair<string, T>>)_dictionary).Contains(item);
        }

        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, T>>)_dictionary).CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
