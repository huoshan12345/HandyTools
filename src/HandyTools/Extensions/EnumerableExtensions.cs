﻿namespace HandyTools.Extensions;

public static class EnumerableExtensions
{    
    public static string JoinWith<T>(this IEnumerable<T> enumerable, string? separator)
    {
        return string.Join(separator, enumerable);
    }
}