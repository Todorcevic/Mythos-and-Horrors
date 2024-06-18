using System;
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public static class LinqExtensions
    {
        public static T Unique<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        {
            T[] results = source.Distinct(comparer ?? EqualityComparer<T>.Default).Take(2).ToArray();
            return results.Length == 1 ? results[0] : throw new InvalidOperationException("Sequence contains more than one unique element");
        }

        public static T UniqueOrDefault<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        {
            T[] results = source.Distinct(comparer ?? EqualityComparer<T>.Default).Take(2).ToArray();
            return results.Length == 1 ? results[0] : default;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source) action(item);
            return source;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int num = 0;
            foreach (T item in source) action(item, num++);
            return source;
        }

        public static int IndexOf<T>(this IEnumerable<T> source, T item)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            int index = 0;
            foreach (var element in source)
            {
                if (EqualityComparer<T>.Default.Equals(element, item)) return index;
                index++;
            }
            return -1;
        }

        public static T NextElementFor<T>(this IEnumerable<T> source, T item)
        {
            if (source == null || item == null) return default;
            return source.ElementAtOrDefault(source.IndexOf(item) + 1);
        }

        public static T Rand<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.OrderBy(element => Guid.NewGuid()).FirstOrDefault();
        }

        public static IEnumerable<T> Rand<T>(this IEnumerable<T> source, int amount)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            return source.OrderBy(element => Guid.NewGuid()).Take(amount);
        }
    }
}