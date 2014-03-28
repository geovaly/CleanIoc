using System;
using System.Collections.Generic;

namespace CleanIoc.Utility
{
    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action.Invoke(item);
            }
        }
    }
}
