using System.Collections.Generic;

namespace FfmpegVideoMerger.Logic; 

public static class PairUtils {

    public static KeyValuePair<T1, T2> To<T1, T2>(this T1 self, T2 another) {
        return new KeyValuePair<T1, T2>(self, another);
    }
}