using System;
using System.IO;

namespace FfmpegVideoMerger.Logic.Storage; 

public static class StoragePathsProvider {

    public static string GetDataPath() {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FfmpegVideoMerger"
        );
    }
}