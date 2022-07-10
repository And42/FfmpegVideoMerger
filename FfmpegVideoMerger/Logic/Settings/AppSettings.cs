using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FfmpegVideoMerger.Logic.Settings; 

public class AppSettings {

    public enum Theme {
        Light,
        Dark
    }
    
    public enum Language {
        English,
        Russian
    }

    private readonly string _filePath;

    public Theme AppTheme {
        get => _appTheme;
        set => SetProperty(ref _appTheme, value);
    }
    private Theme _appTheme;

    public Language AppLanguage {
        get => _appLanguage;
        set => SetProperty(ref _appLanguage, value);
    }
    private Language _appLanguage;
    
    public AppSettings(string filePath) {
        _filePath = filePath;
        
        Load();
    }
    
    private void SetProperty<T>(ref T storage, T value) {
        if (EqualityComparer<T>.Default.Equals(storage, value)) {
            return;
        }

        storage = value;
        Save();
    }

    private void Load() {
        if (!File.Exists(_filePath)) {
            return;
        }

        var document = File.OpenRead(_filePath).Use(it => JsonDocument.Parse(it));
        _appTheme = document.RootElement.GetProperty(SettingsJson.ThemeKey).GetString() switch {
            SettingsJson.ThemeLightValue => Theme.Light,
            SettingsJson.ThemeDarkValue => Theme.Dark,
            _ => Theme.Light
        };
        _appLanguage = document.RootElement.GetProperty(SettingsJson.LanguageKey).GetString() switch {
            SettingsJson.LanguageEnglishValue => Language.English,
            SettingsJson.LanguageRussianValue => Language.Russian,
            _ => Language.English
        };
    }

    private void Save() {
        string? parentDirectory = Path.GetDirectoryName(_filePath);
        if (parentDirectory.IsNotNullOrEmpty() && !Directory.Exists(parentDirectory)) {
            Directory.CreateDirectory(parentDirectory!);
        }

        var jsonOptions = new JsonWriterOptions {
            Indented = true
        };

        using (var output = File.Create(_filePath))
        using (var writer = new Utf8JsonWriter(output, jsonOptions)) {
            writer.WriteStartObject();
            
            writer.WriteString(SettingsJson.ThemeKey, AppTheme switch {
                Theme.Light => SettingsJson.ThemeLightValue,
                Theme.Dark => SettingsJson.ThemeDarkValue,
                _ => throw new ArgumentException(nameof(AppTheme))
            });
            writer.WriteString(SettingsJson.LanguageKey, AppLanguage switch {
                Language.English => SettingsJson.LanguageEnglishValue,
                Language.Russian => SettingsJson.LanguageRussianValue,
                _ => throw new ArgumentException(nameof(AppLanguage))
            });
            
            writer.WriteEndObject();
        }
    }

    private static class SettingsJson {
        public const string ThemeKey = "theme";
        public const string ThemeLightValue = "light";
        public const string ThemeDarkValue = "dark";
        public const string LanguageKey = "language";
        public const string LanguageEnglishValue = "en";
        public const string LanguageRussianValue = "ru";
    }
}