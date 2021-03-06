﻿using Sels.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sels.Core.Extensions
{
    public static class CollectionExtensions
    {
        private static Random _random = new Random();

        #region IEnumerable
        public static IEnumerable<T> Copy<T>(this IEnumerable<T> list)
        {
            return new List<T>(list);
        }

        public static bool AreAllUnique<T>(this IEnumerable<T> list)
        {
            if (list.HasValue())
            {
                foreach(var item in list)
                {
                    var occuranceAmount = 0;
                    foreach(var itemToCompare in list)
                    {
                        if (item.Equals(itemToCompare))
                        {
                            occuranceAmount++;
                        }

                        // Has to be 2 because an item counts itself at least once
                        if(occuranceAmount > 1)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        #region Random
        public static int GetRandomIndex<T>(this IEnumerable<T> value)
        {
            return _random.Next(0, value.Count());
        }

        public static T GetAtIndex<T>(this IEnumerable<T> value, int index)
        {
            var currentIndex = 0;
            foreach (var item in value)
            {
                if (currentIndex == index)
                {
                    return item;
                }

                currentIndex++;
            }

            return default;
        }

        public static T GetRandomItem<T>(this IEnumerable<T> value)
        {
            var randomIndex = value.GetRandomIndex();
            return value.GetAtIndex(randomIndex);
        }
        #endregion
        #endregion

        #region Dictionary
        public static void Merge<TKey, TItem>(this Dictionary<TKey, List<TItem>> dictionary, Dictionary<TKey, List<TItem>> dictionaryToMerge)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            dictionaryToMerge.ValidateVariable(nameof(dictionaryToMerge));

            foreach(var pair in dictionaryToMerge)
            {
                dictionary.AddValues(pair.Key, pair.Value);
            }
        }

        #region AddValue
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));

            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        [Obsolete]
        public static void AddValueToCollection<TKey, TItem>(this Dictionary<TKey, IEnumerable<TItem>> dictionary, TKey key, TItem item)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));

            if (dictionary.ContainsKey(key))
            {
                var newList = new List<TItem>(dictionary[key]);
                newList.Add(item);
                dictionary[key] = newList;
            }
            else
            {
                var newList = new List<TItem>();
                newList.Add(item);
                dictionary.Add(key, newList);
            }
        }

        /// <summary>
        /// Adds <paramref name="item"/> to the list of <paramref name="key"/> if <paramref name="key"/> exists in <paramref name="dictionary"/>, otherwise create new list and add <paramref name="item"/> to it.
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TItem">Collection type</typeparam>
        /// <param name="dictionary">Dictionary to add item to</param>
        /// <param name="key">Key for list</param>
        /// <param name="item">Item to add</param>
        public static void AddValueToList<TKey, TItem>(this Dictionary<TKey, List<TItem>> dictionary, TKey key, TItem item)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));

            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(item);
            }
            else
            {
                var newList = new List<TItem>();
                newList.Add(item);
                dictionary.Add(key, newList);
            }
        }

        [Obsolete]
        public static void AddValues<TKey, TItem>(this Dictionary<TKey, IEnumerable<TItem>> dictionary, TKey key, IEnumerable<TItem> items)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));
            items.ValidateVariable(nameof(items));

            if (dictionary.ContainsKey(key))
            {
                var newList = new List<TItem>(dictionary[key]);
                newList.AddRange(items);
                dictionary[key] = newList;
            }
            else
            {
                dictionary.Add(key, items);
            }
        }

        /// <summary>
        /// Adds <paramref name="items"/> to the list of <paramref name="key"/> if <paramref name="key"/> exists in <paramref name="dictionary"/>, otherwise create new list and add <paramref name="items"/> to it.
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TItem">Collection type</typeparam>
        /// <param name="dictionary">Dictionary to add item to</param>
        /// <param name="key">Key for list</param>
        /// <param name="items">Items to add</param>
        public static void AddValues<TKey, TItem>(this Dictionary<TKey, List<TItem>> dictionary, TKey key, IEnumerable<TItem> items)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));
            items.ValidateVariable(nameof(items));

            if (dictionary.ContainsKey(key))
            {
                dictionary[key].AddRange(items);
            }
            else
            {
                dictionary.Add(key, new List<TItem>(items));
            }
        }
        #endregion

        #region TryGetOrSet
        public static TValue TryGetOrSet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));

            return dictionary.TryGetOrSet(key, () => value);
        }

        public static TValue TryGetOrSet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFunc)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                var value = valueFunc();
                dictionary.Add(key, value);
                return value;
            }
        }

        public static TValue TryGetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));

            return dictionary.TryGetOrSet(key, default(TValue));
        }

        #endregion

        #region ContainsItem
        public static bool ContainsItem<TKey, TItem>(this Dictionary<TKey, IEnumerable<TItem>> dictionary, TKey key, TItem item)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));
            item.ValidateVariable(nameof(item));

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key].Contains(item);
            }

            return false;
        }

        public static bool ContainsItem<TKey, TItem>(this Dictionary<TKey, List<TItem>> dictionary, TKey key, TItem item)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));
            item.ValidateVariable(nameof(item));

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key].Contains(item);
            }

            return false;
        }

        public static bool ContainsItem<TKey, TItem>(this Dictionary<TKey, Collection<TItem>> dictionary, TKey key, TItem item)
        {
            dictionary.ValidateVariable(nameof(dictionary));
            key.ValidateVariable(nameof(key));
            item.ValidateVariable(nameof(item));

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key].Contains(item);
            }

            return false;
        }
        #endregion
        #endregion

        #region Grid
        public static int GetColumnLength<T>(this IEnumerable<IEnumerable<T>> table)
        {
            var biggestLength = 0;
            foreach (var row in table)
            {
                var rowCount = row.Count();

                if (rowCount > biggestLength)
                {
                    biggestLength = rowCount;
                }
            }

            return biggestLength;
        }

        public static int GetColumnLength<T>(this List<List<T>> table)
        {
            var biggestLength = 0;
            foreach (var row in table)
            {
                var rowCount = row.Count;

                if (rowCount > biggestLength)
                {
                    biggestLength = rowCount;
                }
            }

            return biggestLength;
        }        
        #endregion

        #region Manipulation
        public static IList<T> UpdateFirst<T>(this IList<T> source, Func<T, T> valueUpdater)
        {
            valueUpdater.ValidateVariable(nameof(valueUpdater));

            var oldValue = source.FirstOrDefault();

            if (oldValue != null)
            {
                var newValue = valueUpdater(oldValue);

                source.Remove(oldValue);
                source.Insert(0, newValue);
            }

            return source;
        }

        public static ICollection<T> UpdateLast<T>(this ICollection<T> source, Func<T, T> valueUpdater)
        {
            valueUpdater.ValidateVariable(nameof(valueUpdater));

            var oldValue = source.LastOrDefault();

            if (oldValue != null)
            {
                var newValue = valueUpdater(oldValue);

                source.Remove(oldValue);
                source.Add(newValue);
            }

            return source;
        }

        public static ICollection<T> RemoveLast<T>(this ICollection<T> source)
        {
            var oldValue = source.LastOrDefault();

            if (oldValue != null)
            {
                source.Remove(oldValue);
            }

            return source;
        }

        public static ICollection<T> RemoveFirst<T>(this ICollection<T> source)
        {
            var oldValue = source.FirstOrDefault();

            if (oldValue != null)
            {
                source.Remove(oldValue);
            }

            return source;
        }
        #endregion
    }
}
