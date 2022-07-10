using System.IO;

namespace FfmpegVideoMerger.Logic; 

public static class StreamUtils {

    public static StreamReader Reader(this Stream self) {
        return new StreamReader(self);
    }
}