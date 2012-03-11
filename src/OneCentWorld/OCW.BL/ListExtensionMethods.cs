using System;
using System.Collections.Generic;

namespace OCW.BL
{
    public static class ListExtensionMethods
    {
        private static readonly Random random = new Random();

        public static T Random<T>(this IList<T> list)
        {
            return list.Count == 0 ? default(T) : list[random.Next(0, list.Count)];
        }
    }
}
