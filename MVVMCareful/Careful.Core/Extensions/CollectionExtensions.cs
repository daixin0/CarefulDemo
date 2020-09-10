using Careful.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Careful.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static Collection<T> AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var each in items)
            {
                collection.Add(each);
            }

            return collection;
        }
        [Pure]
        public static void CountForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            Contract.Requires(source != null);
            Contract.Requires(action != null);

            int i = 0;
            source.ForEach(item => action(item, i++));
        }

        [Pure]
        public static IEnumerable<TTarget> CountSelect<TSource, TTarget>(this IEnumerable<TSource> source, Func<TSource, int, TTarget> func)
        {
            int i = 0;
            foreach (var item in source)
            {
                yield return func(item, i++);
            }
        }

        [Pure]
        public static bool AllUnique<T>(this IList<T> source)
        {
            Contract.Requires<ArgumentNullException>(source != null);

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            return source.TrueForAllPairs((a, b) => !comparer.Equals(a, b));
        }

        [Pure]
        public static bool TrueForAllPairs<T>(this IList<T> source, Func<T, T, bool> compare)
        {
            Contract.Requires(source != null);
            Contract.Requires(compare != null);

            for (int i = 0; i < source.Count; i++)
            {
                for (int j = i + 1; j < source.Count; j++)
                {
                    if (!compare(source[i], source[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        [Pure]
        public static bool TrueForAllAdjacentPairs<T>(this IEnumerable<T> source, Func<T, T, bool> compare)
        {
            Contract.Requires(source != null);
            Contract.Requires(compare != null);

            return source.SelectAdjacentPairs().All(t => compare(t.Item1, t.Item2));
        }

        public static IEnumerable<Tuple<T, T>> SelectAdjacentPairs<T>(this IEnumerable<T> source)
        {
            Contract.Requires(source != null);
            bool hasPrevious = false;
            T previous = default(T);

            foreach (var item in source)
            {
                if (!hasPrevious)
                {
                    previous = item;
                    hasPrevious = true;
                }
                else
                {
                    yield return Tuple.Create(previous, item);
                    previous = item;
                }
            }
        }

        [Pure]
        public static bool AllNotNullOrEmpty(this IEnumerable<string> source)
        {
            Contract.Requires(source != null);
            return source.All(item => !string.IsNullOrEmpty(item));
        }

        [Pure]
        public static bool AllExistIn<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> set)
        {
            Contract.Requires(source != null);
            Contract.Requires(set != null);

            return source.All(item => set.Contains(item));
        }

        [Pure]
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        {
            Contract.Requires(source != null);

            if (source is ICollection<TSource>)
            {
                return ((ICollection<TSource>)source).Count == 0;
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    return !enumerator.MoveNext();
                }
            }
        }

        [Pure]
        public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Contract.Requires(source != null);
            Contract.Requires(predicate != null);

            int index = 0;
            foreach (TSource item in source)
            {
                if (predicate(item))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public static ReadOnlyCollection<TSource> ToReadOnlyCollection<TSource>(this IEnumerable<TSource> source)
        {
            Contract.Requires(source != null);
            Contract.Ensures(Contract.Result<ReadOnlyCollection<TSource>>() != null);
            return new ReadOnlyCollection<TSource>(source.ToArray());
        }

        
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            Contract.Requires(source != null);
            Contract.Requires(action != null);

            foreach (TSource item in source)
            {
                action(item);
            }
        }

        public static TSource RemoveLast<TSource>(this IList<TSource> source)
        {
            Contract.Requires(source != null);
            Contract.Requires(source.Count > 0);
            TSource item = source[source.Count - 1];
            source.RemoveAt(source.Count - 1);
            return item;
        }

        public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> source)
        {
            return source ?? Enumerable.Empty<TSource>();
        }
        public static IEnumerable<TSource> SelectRecursive<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TSource>> recursiveSelector)
        {
            Contract.Requires(source != null);
            Contract.Requires(recursiveSelector != null);

            var stack = new Stack<IEnumerator<TSource>>();
            stack.Push(source.GetEnumerator());

            try
            {
                while (stack.Any())
                {
                    if (stack.Peek().MoveNext())
                    {
                        var current = stack.Peek().Current;

                        yield return current;

                        stack.Push(recursiveSelector(current).GetEnumerator());
                    }
                    else
                    {
                        stack.Pop().Dispose();
                    }
                }
            }
            finally
            {
                while (stack.Any())
                {
                    stack.Pop().Dispose();
                }
            }
        }


        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, params T[] items)
        {
            return source.Concat(items.AsEnumerable());
        }

        [Pure]
        public static bool Contains<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            return dictionary.Contains(new KeyValuePair<TKey, TValue>(key, value));
        }

        [Pure]
        public static bool CountAtLeast<T>(this IEnumerable<T> source, int count)
        {
            Contract.Requires(source != null);
            if (source is ICollection<T>)
            {
                return ((ICollection<T>)source).Count >= count;
            }
            else
            {
                using (var enumerator = source.GetEnumerator())
                {
                    while (count > 0)
                    {
                        if (enumerator.MoveNext())
                        {
                            count--;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        public static IEnumerable<TSource> Except<TSource, TOther>(this IEnumerable<TSource> source, IEnumerable<TOther> other, Func<TSource, TOther, bool> comparer)
        {
            return from item in source
                   where !other.Any(x => comparer(item, x))
                   select item;
        }

        public static IEnumerable<TSource> Intersect<TSource, TOther>(this IEnumerable<TSource> source, IEnumerable<TOther> other, Func<TSource, TOther, bool> comparer)
        {
            return from item in source
                   where other.Any(x => comparer(item, x))
                   select item;
        }

        public static INotifyCollectionChanged AsINPC<T>(this ReadOnlyObservableCollection<T> source)
        {
            Contract.Requires(source != null);
            return (INotifyCollectionChanged)source;
        }

        
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            Contract.Requires(source != null);
            return new ObservableCollection<T>(source);
        }

        public static void Synchronize<TSource, TTarget>(this ObservableCollectionSynchronize<TTarget> targetCollection, IList<TSource> sourceCollection, Func<TSource, TTarget, bool> matcher, Func<TSource, TTarget> mapper) where TSource : IEquatable<TSource>
        {
            Contract.Requires(targetCollection != null);
            Contract.Requires(mapper != null);
            Contract.Requires(sourceCollection != null);
            Contract.Requires(sourceCollection.AllUnique());

            using (targetCollection.BeginMultiUpdate())
            {
                for (int i = 0; i < sourceCollection.Count; i++)
                {
                    var sourceItem = sourceCollection[i];
                    var targetIndex = targetCollection.IndexOf(targetItem => matcher(sourceItem, targetItem));
                    if (targetIndex >= 0)
                    {
                        if (targetIndex != i)
                        {
                            Debug.Assert(targetIndex > i, "this would only happen if we have duplicates...which we should never have!");
                            targetCollection.Move(targetIndex, i);
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        var newItem = mapper(sourceItem);
                        Debug.Assert(matcher(sourceItem, newItem));
                        targetCollection.Insert(i, newItem);
                    }
                }

                while (targetCollection.Count > sourceCollection.Count)
                {
                    targetCollection.RemoveLast();
                }
            }
        }

        public static IDictionary<TKey, TValue> Clone<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");
            return source.ToDictionary(p => p.Key, p => p.Value);
        }

        public static bool TryGetTypedValue<TOutput, TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TOutput value) where TOutput : TValue
        {
            Contract.Requires(dictionary != null);
            TValue val;
            if (dictionary.TryGetValue(key, out val))
            {
                if (val is TOutput)
                {
                    value = (TOutput)val;
                    return true;
                }
            }
            value = default(TOutput);
            return false;
        }

        public static TValue EnsureItem<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(valueFactory != null);
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = valueFactory();
                dictionary.Add(key, value);
            }
            return value;
        }
    }
}
