using System.IO;
using FfmpegVideoMerger.Logic.Storage;

namespace FfmpegVideoMerger.Logic.Settings; 

public static class SettingsProvider {

    public static AppSettings LoadSettings() {
        string dataFolder = StoragePathsProvider.GetDataPath();

        if (!Directory.Exists(dataFolder)) {
            Directory.CreateDirectory(dataFolder);
        }

        string filePath = Path.Combine(dataFolder, "Settings.json");

        return new AppSettings(filePath);
    }
}