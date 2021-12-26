using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CustomDictionary
{
    public class CustomDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private LinkedList<KeyValuePair<TKey, TValue>>[] _hashTable;
        
        public CustomDictionary(int size = 16)
        {
            _hashTable = new LinkedList<KeyValuePair<TKey, TValue>>[size];

            Count = 0;
            
            Keys = new List<TKey>();
            Values = new List<TValue>(); 
            
            for (int i = 0; i < _hashTable.Length; ++i)
            {
                _hashTable[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }
        }
        
        public int Count { get; set; }

        public ICollection<TKey> Keys { get; set; }

        public ICollection<TValue> Values { get; set; }
        
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                int hash = Math.Abs(key.GetHashCode()) % _hashTable.Length;

                if (ContainsKey(key))
                {
                    TValue result = default(TValue);

                    foreach (var item in _hashTable[hash])
                    {
                        if (key.Equals(item.Key))
                        {
                            result = item.Value;
                        }
                    }

                    return result;
                }
                else
                {
                    throw new Exception($"Element with key {key} not found.");
                }
            }

            set
            {
                int hash = Math.Abs(key.GetHashCode()) % _hashTable.Length;
                
                if (ContainsKey(key))
                {
                    for (var cur = _hashTable[hash].First; !cur.Equals(_hashTable[hash].Last); cur = cur.Next)
                    {
                        if (key.Equals(cur.Value.Key))
                        {
                            Values.Remove(cur.Value.Value);
                            LinkedListNode<KeyValuePair<TKey, TValue>> newNode = new LinkedListNode<KeyValuePair<TKey, TValue>>(new KeyValuePair<TKey, TValue>(key, value));
                            cur = newNode;
                            Values.Add(value);
                        }
                    }
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        private void IncreaseHashTable()
        {
            LinkedList<KeyValuePair<TKey, TValue>>[] newHashTable = new LinkedList<KeyValuePair<TKey, TValue>>[_hashTable.Length * 2];

            for (int i = 0; i < newHashTable.Length; ++i)
            {
                newHashTable[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }

            for (int i = 0; i < _hashTable.Length; ++i)
            {
                foreach (var item in _hashTable[i])
                {
                    int hash = Math.Abs(item.Key.GetHashCode()) % newHashTable.Length;

                    newHashTable[hash].AddLast(item);
                }
            }

            _hashTable = newHashTable;
        }

        public void Add(TKey key, TValue value)
        {
            int hash = Math.Abs(key.GetHashCode()) % _hashTable.Length;

            if (!ContainsKey(key))
            {
                Count++;
                bool flag = true;
                foreach (var item in _hashTable)
                {
                    if (item.Count == 0)
                    {
                        flag = false;
                    }
                }

                if (flag || Count >= _hashTable.Length)
                {
                    IncreaseHashTable();
                }
                
                _hashTable[hash].AddLast(new KeyValuePair<TKey, TValue>(key, value));
                
                Keys.Add(key);
                Values.Add(value);
            }
            else
            {
                throw new Exception($"There's already an element with key: {key}");
            }
        }

        public bool ContainsKey(TKey key)
        {
            int hash = Math.Abs(key.GetHashCode()) % _hashTable.Length;

            var keys = _hashTable[hash].Select(item => item.Key);

            return keys.Contains(key);
        }

        public bool Remove(TKey key)
        {
            int hash = Math.Abs(key.GetHashCode()) % _hashTable.Length;

            if (ContainsKey(key))
            {
                Count--;

                foreach (var item in _hashTable[hash])
                {
                    if (key.Equals(item.Key))
                    {
                        _hashTable[hash].Remove(item);
                        Keys.Remove(item.Key);
                        Values.Remove(item.Value);
                        break;
                    }
                }

                return true;
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int hash = Math.Abs(key.GetHashCode()) % _hashTable.Length;

            if (ContainsKey(key))
            {
                foreach (var item in _hashTable[hash])
                {
                    value = item.Value;
                    return true;
                }
            }

            value = default(TValue);

            return false;
        }

        public void Clear()
        {
            int size = 16;
            _hashTable = new LinkedList<KeyValuePair<TKey, TValue>>[size];
            Keys.Clear();
            Values.Clear();
            
            for (int i = 0; i < _hashTable.Length; ++i)
            {
                _hashTable[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }

            Count = 0;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex - 1 < Count)
            {
                throw new Exception("There's no space to insert elements into array.");
            }

            if (array == null)
            {
                throw new NullReferenceException($"Array 'array' has null reference.");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentException("Negative argument 'arrayIndex' is not valid.");
            }

            List<KeyValuePair<TKey, TValue>> allPairs = new List<KeyValuePair<TKey, TValue>>();

            for (int i = 0; i < _hashTable.Length; ++i)
            {
                foreach (var item in _hashTable[i])
                {
                    allPairs.Add(item);
                }
            }

            int curIndex = 0;
            for (int i = arrayIndex; i < array.Length; ++i)
            {
                array[i] = allPairs[curIndex];
                curIndex++;
            }
        }
        
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }
        
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }
        
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < _hashTable.Length; ++i)
            {
                foreach (var item in _hashTable[i])
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}