using System;
using FfmpegVideoMerger.Logic.Settings;
using FfmpegVideoMerger.UI.Main.MultipleFiles;
using FfmpegVideoMerger.UI.Main.Settings;
using SingleFileViewModel = FfmpegVideoMerger.UI.Main.SingleFile.SingleFileViewModel;

namespace FfmpegVideoMerger.Logic.DI; 

public class AppComponent : IAppComponent {
    private readonly Lazy<AppSettings> _appSettings;
    public AppSettings AppSettings => _appSettings.Value;

    public SettingsViewModel SettingsViewModel => new(AppSettings);

    public SingleFileViewModel SingleFileViewModel => new(AppSettings);

    public MultipleFilesViewModel MultipleFilesViewModel => new(AppSettings);

    public AppComponent() {
        _appSettings = new Lazy<AppSettings>(SettingsProvider.LoadSettings);
    }
}