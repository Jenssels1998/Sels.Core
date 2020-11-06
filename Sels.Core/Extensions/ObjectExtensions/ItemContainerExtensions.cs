﻿using Sels.Core.Extensions.General.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sels.Core.Extensions.Object.ItemContainer
{
    public static class ItemContainerExtensions
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

        #region List
        #region Manipulation
        public static bool UpdateFirst<T>(this List<T> list, Func<T, T, bool> comparator, T value)
        {
            return list.UpdateItemInEnumerable(comparator, value, true);
        }

        public static bool UpdateAll<T>(this List<T> list, Func<T, T, bool> comparator, T value)
        {
            return list.UpdateItemInEnumerable(comparator, value, false);
        }

        public static bool DeleteFirst<T>(this List<T> list, Func<T, T, bool> comparator, T value)
        {
            return list.DeleteItemInEnumerable(comparator, value, true);
        }

        public static bool DeleteAll<T>(this List<T> list, Func<T, T, bool> comparator, T value)
        {
            return list.DeleteItemInEnumerable(comparator, value, false);
        }
        #region Privates
        public static bool UpdateItemInEnumerable<T>(this List<T> list, Func<T, T, bool> comparator, T value, bool onlyFirst = false)
        {
            var isUpdated = false;

            if (list.HasValue())
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];

                    if (item.HasValue() && value.HasValue() && comparator(value, item))
                    {
                        list[i] = value;

                        isUpdated = true;

                        if (onlyFirst)
                        {
                            return isUpdated;
                        }
                    }
                }
            }

            return isUpdated;
        }

        public static bool DeleteItemInEnumerable<T>(this List<T> list, Func<T, T, bool> comparator, T value, bool onlyFirst = false)
        {
            var hasDeleted = false;

            if (list.HasValue())
            {
                foreach (var item in list)
                {
                    if (item.HasValue() && value.HasValue() && comparator(value, item))
                    {
                        list.Remove(item);
                        hasDeleted = true;

                        if (onlyFirst)
                        {
                            break;
                        }
                    }
                }
            }

            return hasDeleted;
        }
        #endregion
        #endregion
        #endregion
    }
}