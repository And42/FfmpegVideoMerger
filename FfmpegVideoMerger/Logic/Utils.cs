using System;

namespace FfmpegVideoMerger.Logic; 

public static class Utils {

    public static T As<T>(this object self) where T : class {
        return self as T ?? throw new InvalidOperationException();
    }
    
    public static void Use<T>(this T resource, Action<T> action) where T : IDisposable {
        using (resource) {
            action(resource);
        }
    }
    
    public static R Use<T, R>(this T resource, Func<T, R> action) where T : IDisposable {
        using (resource) {
            return action(resource);
        }
    }
}