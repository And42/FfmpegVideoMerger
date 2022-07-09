using System;
using System.Collections.Generic;
using System.Linq;

namespace FfmpegVideoMerger.Logic; 

public static class CollectionUtils {

    public static bool IsEmpty<T>(this IReadOnlyList<T> self) {
        return self.Count == 0;
    }
    
    public static bool IsNotEmpty<T>(this IReadOnlyList<T> self) {
        return !self.IsEmpty();
    }

    public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
        return !source.Any(predicate);
    }

    public static TOutput[] ConvertAll<TInput, TOutput>(this TInput[] self, Converter<TInput, TOutput> converter) {
        return Array.ConvertAll(self, converter);
    }
}