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
}