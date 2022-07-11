using System;
using System.Collections.Generic;
using FfmpegVideoMerger.Logic;
using FfmpegVideoMerger.Logic.Language;
using FfmpegVideoMerger.Logic.Settings;
using FfmpegVideoMerger.UI.Base;
using Microsoft.Win32;

namespace FfmpegVideoMerger.UI.Main.Settings; 

public class SettingsViewModel : ViewModel {

    private readonly AppSettings _appSettings;
    
    public bool IsDark {
        get => _isDark;
        set => SetProperty(ref _isDark, value);
    }
    private bool _isDark;

    public IReadOnlyList<string> Languages { get; }

    public int CurrentLanguageIndex {
        get => _currentLanguageIndexIndex;
        set => SetProperty(ref _currentLanguageIndexIndex, value);
    }
    private int _currentLanguageIndexIndex;

    public bool CheckForUpdates {
        get => _checkForUpdates;
        set => SetProperty(ref _checkForUpdates, value);
    }
    private bool _checkForUpdates;

    public string FfmpegPath {
        get => _ffmpegPath;
        set => SetProperty(ref _ffmpegPath, value);
    }
    private string _ffmpegPath;
    
    public ActionCommand ChooseFfmpegPathCommand { get; }

    public SettingsViewModel() {
        _appSettings = SettingsProvider.LoadSettings();

        _isDark = _appSettings.AppTheme == AppSettings.Theme.Dark;

        Languages = new[] {
            "English",
            "Русский"
        };

        _currentLanguageIndexIndex = _appSettings.AppLanguage switch {
            AppSettings.Language.English => 0,
            AppSettings.Language.Russian => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(_appSettings.AppLanguage), _appSettings.AppLanguage, null)
        };

        _checkForUpdates = _appSettings.CheckForUpdates;
        _ffmpegPath = _appSettings.FfmpegPath;

        ChooseFfmpegPathCommand = new ActionCommand(ChooseFfmpegPathCommandExecute, canExecute: () => true);
    }

    private void ChooseFfmpegPathCommandExecute() {
        var openDialog = new OpenFileDialog {
            Filter = "*.exe|*.exe"
        };
        if (openDialog.ShowDialog() != true) {
            return;
        }

        FfmpegPath = openDialog.FileName;
    }

    protected override void OnPropertyChanged(string? propertyName = null) {
        base.OnPropertyChanged(propertyName);

        switch (propertyName) {
            case nameof(CurrentLanguageIndex):
                var language = CurrentLanguageIndex switch {
                    1 => AppSettings.Language.Russian,
                    _ => AppSettings.Language.English
                };

                _appSettings.AppLanguage = language;
                LanguageUtils.SetLanguage(language);
                var activeWindow = MainWindow.ActiveInstance;
                var newWindow = new MainWindow();
                newWindow.Show();
                newWindow.ViewModel.GoToPage(MainViewModel.Page.Settings);
                activeWindow?.Close();
                break;
            case nameof(IsDark):
                _appSettings.AppTheme = IsDark ? AppSettings.Theme.Dark : AppSettings.Theme.Light;
                ThemeUtils.SetTheme(_appSettings.AppTheme);
                break;
            case nameof(CheckForUpdates):
                _appSettings.CheckForUpdates = CheckForUpdates;
                break;
            case nameof(FfmpegPath):
                _appSettings.FfmpegPath = FfmpegPath;
                break;
        }
    }
}