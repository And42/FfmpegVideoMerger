using FfmpegVideoMerger.Logic.Settings;
using FfmpegVideoMerger.UI.Main.MultipleFiles;
using FfmpegVideoMerger.UI.Main.Settings;
using SingleFileViewModel = FfmpegVideoMerger.UI.Main.SingleFile.SingleFileViewModel;

namespace FfmpegVideoMerger.Logic.DI; 

public interface IAppComponent {
    AppSettings AppSettings { get; }

    SettingsViewModel SettingsViewModel { get; }
    SingleFileViewModel SingleFileViewModel { get; }
    MultipleFilesViewModel MultipleFilesViewModel { get; }
}