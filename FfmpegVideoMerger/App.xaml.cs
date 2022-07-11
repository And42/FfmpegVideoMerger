using System.Windows;
using FfmpegVideoMerger.Logic;
using FfmpegVideoMerger.Logic.Language;
using FfmpegVideoMerger.Logic.Settings;
using FfmpegVideoMerger.Logic.Versioning;

namespace FfmpegVideoMerger; 

public partial class App {
    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        var settings = SettingsProvider.LoadSettings();
        ThemeUtils.SetTheme(settings.AppTheme);
        LanguageUtils.SetLanguage(settings.AppLanguage);
        if (settings.CheckForUpdates) {
            NewVersionChecker.CheckInBackground();
        }
    }
}