using System;
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public static class LinqExtension
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
    }
}