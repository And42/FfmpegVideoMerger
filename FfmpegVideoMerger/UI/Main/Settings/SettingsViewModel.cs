using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using AdonisUI;
using FfmpegVideoMerger.UI.Base;

namespace FfmpegVideoMerger.UI.Main.Settings; 

public class SettingsViewModel : ViewModel {

    public IReadOnlyList<string> Languages { get; }

    public int CurrentLanguageIndex {
        get => _currentLanguageIndexIndex;
        set => SetProperty(ref _currentLanguageIndexIndex, value);
    }
    private int _currentLanguageIndexIndex;

    public bool IsDark {
        get => _isDark;
        set => SetProperty(ref _isDark, value);
    }
    private bool _isDark;

    public SettingsViewModel() {
        Languages = new[] {
            "English",
            "Русский"
        };

        _currentLanguageIndexIndex = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName switch {
            "ru" => 1,
            _ => 0
        };
    }

    protected override void OnPropertyChanged(string? propertyName = null) {
        base.OnPropertyChanged(propertyName);

        switch (propertyName) {
            case nameof(CurrentLanguageIndex):
                string language = CurrentLanguageIndex switch {
                    1 => "ru",
                    _ => "en"
                };

                var cultureInfo = CultureInfo.GetCultureInfo(language);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
                var activeWindow = MainWindow.ActiveInstance;
                new MainWindow().Show();
                activeWindow?.Close();
                break;
            case nameof(IsDark):
                var colorScheme = IsDark ? ResourceLocator.DarkColorScheme : ResourceLocator.LightColorScheme;
                ResourceLocator.SetColorScheme(Application.Current.Resources, colorScheme);
                break;
        }
    }
}