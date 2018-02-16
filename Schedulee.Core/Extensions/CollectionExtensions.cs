using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Schedulee.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static void InsertSorted<T>(this IList<T> list, IEnumerable<T> items, Comparison<T> comparer = null)
        {
            if(items == null) return;
            list = list ?? new List<T>();
            if(comparer == null)
            {
                foreach(var obj in items)
                {
                    list.Add(obj);
                }
            }
            else
            {
                var sorted = items.ToList();
                sorted.Sort(comparer);
                int first = 0, second = 0;
                while(second < sorted.Count)
                {
                    if(first >= list.Count)
                    {
                        list.Add(sorted[second++]);
                    }
                    else if(comparer.Invoke(list[first], sorted[second]) > 0)
                    {
                        list.Insert(first++, sorted[second++]);
                    }
                    else if(comparer.Invoke(list[first], sorted[second]) == 0)
                    {
                        first++;
                        second++;
                    }
                    else
                    {
                        first++;
                    }
                }
            }
        }

        public static int Count(this IEnumerable enumerable)
        {
            if(enumerable == null) return 0;

            var itemsList = enumerable as ICollection;
            if(itemsList != null)
            {
                return itemsList.Count;
            }

            var enumerator = enumerable.GetEnumerator();
            var count = 0;
            while(enumerator.MoveNext())
            {
                count++;
            }

            return count;
        }

        public static object ElementAt(this IEnumerable items, int position)
        {
            if(items == null) return null;

            var itemsList = items as IList;
            if(itemsList != null) return itemsList[position];

            var enumerator = items.GetEnumerator();
            for(var index = 0; index <= position; index++)
            {
                enumerator.MoveNext();
            }

            return enumerator.Current;
        }

        public static T GetItemAtIndex<T>(this IEnumerable<T> items, int position)
        {
            var item = items.ElementAt(position);
            return item != null ? (T) item : default(T);
        }

        public static int IndexOf(this IEnumerable items, object obj)
        {
            var itemsList = items as IList;
            if(itemsList != null)
            {
                return itemsList.IndexOf(obj);
            }

            int index = 0;
            foreach(var item in items)
            {
                if(item == obj) return index;
                index++;
            }

            return -1;
        }

        public static void AddRange(this IList items, IEnumerable toInsert)
        {
            foreach(var item in toInsert)
            {
                items.Add(item);
            }
        }

        public static void AddRangeGeneric<T>(this IList<T> items, IEnumerable<T> toInsert)
        {
            foreach(var item in toInsert)
            {
                items.Add(item);
            }
        }

        public static void InsertRange(this IList items, int index, IEnumerable toInsert)
        {
            foreach(var item in toInsert)
            {
                items.Insert(index++, item);
            }
        }

        public static object[] ToArray(this IEnumerable items)
        {
            if(items == null) return null;
            var enumerator = items.GetEnumerator();
            var result = new List<object>();
            while(enumerator.MoveNext())
            {
                result.Add(enumerator.Current);
            }

            return result.ToArray();
        }

        public static bool EnumerableEqual(this IEnumerable first, IEnumerable second)
        {
            if(first == null && second == null) return true;
            if(first == null || second == null) return false;
            var enumerator1 = first.GetEnumerator();
            var enumerator2 = second.GetEnumerator();
            do
            {
                var hasNext1 = enumerator1.MoveNext();
                var hasNext2 = enumerator2.MoveNext();
                if(hasNext1 != hasNext2) return false;
                if(hasNext1 == false) return true;
                var value1 = enumerator1.Current;
                var value2 = enumerator2.Current;
                if(!value1.ObjectsEqual(value2)) return false;
            } while(true);
        }

        public static List<T> GetSubList<T>(this IEnumerable<T> collection, int startIndex, int count)
        {
            var list = new List<T>();
            var enumerable = collection as IList<T> ?? collection.ToList();
            count = Math.Min(enumerable.Count, startIndex + count);
            for(int index = startIndex; index < count; index++)
            {
                list.Add(enumerable.GetItemAtIndex(index));
            }

            return list;
        }

        public static List<TDestination> MapTo<TSource, TDestination>(this IEnumerable<TSource> source, Func<TSource, TDestination> mapper)
        {
            return source.Select(mapper.Invoke).ToList();
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach(T item in enumeration)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
        {
            int idx = 0;
            foreach(T item in enumeration)
            {
                action(item, idx++);
            }
        }
    }
}