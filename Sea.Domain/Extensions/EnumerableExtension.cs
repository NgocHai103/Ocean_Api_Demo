using System;
using System.Collections.Generic;

namespace Sea.Domain.Extensions;

public static class EnumerableExtension
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }
}
