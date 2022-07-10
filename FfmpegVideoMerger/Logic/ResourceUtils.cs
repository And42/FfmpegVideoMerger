using System.IO;
using System.Resources;

namespace FfmpegVideoMerger.Logic; 

public static class ResourceUtils {

    public static Stream GetResource<T>(string name) {
        return typeof(T).Assembly.GetManifestResourceStream(typeof(T), name) ?? throw new MissingManifestResourceException(name);
    }

    public static Stream GetAppResource(string name) {
        return GetResource<App>(name);
    }
}