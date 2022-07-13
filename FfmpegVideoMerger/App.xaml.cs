using System;
using System.Windows;
using FfmpegVideoMerger.Logic;
using FfmpegVideoMerger.Logic.DI;
using FfmpegVideoMerger.Logic.Language;
using FfmpegVideoMerger.Logic.Versioning;

namespace FfmpegVideoMerger; 

public partial class App {
    public static IAppComponent AppComponent => _appComponent ?? throw new InvalidOperationException();
    private static IAppComponent? _appComponent;

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        _appComponent = new AppComponent();
        var settings = _appComponent.AppSettings;
        ThemeUtils.SetTheme(settings.AppTheme);
        LanguageUtils.SetLanguage(settings.AppLanguage);
        if (settings.CheckForUpdates) {
            NewVersionChecker.CheckInBackground();
        }
    }
}