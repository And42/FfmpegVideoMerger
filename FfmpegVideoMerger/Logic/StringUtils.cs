namespace FfmpegVideoMerger.Logic; 

public static class StringUtils {

    public static bool IsNullOrEmpty(this string? self) {
        return string.IsNullOrEmpty(self);
    }

    public static bool IsEmpty(this string self) {
        return self.Length == 0;
    }

    public static bool IsNotNullOrEmpty(this string? self) {
        return !self.IsNullOrEmpty();
    }

    public static bool IsNotEmpty(this string self) {
        return !self.IsEmpty();
    }

    public static string Format(this string self, object? arg0, object? arg1) {
        return string.Format(self, arg0, arg1);
    }
}