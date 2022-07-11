using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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

    public bool CheckForUpdates {
        get => _checkForUpdates;
        set => SetProperty(ref _checkForUpdates, value);
    }
    private bool _checkForUpdates;

    public string FfmpegPath {
        get => _ffmpegPath;
        set => SetProperty(ref _ffmpegPath, value);
    }
    private string _ffmpegPath = string.Empty;
    
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

        var settingsJson = File.OpenRead(_filePath).Use(it => JsonSerializer.Deserialize<SettingsJson>(it));
        if (settingsJson == null) {
            return;
        }
        
        _appTheme = settingsJson.Theme switch {
            SettingsJsonValues.ThemeLight => Theme.Light,
            SettingsJsonValues.ThemeDark => Theme.Dark,
            _ => Theme.Light
        };
        _appLanguage = settingsJson.Language switch {
            SettingsJsonValues.LanguageEnglish => Language.English,
            SettingsJsonValues.LanguageRussian => Language.Russian,
            _ => Language.English
        };
        _checkForUpdates = settingsJson.CheckForUpdates ?? true;
        _ffmpegPath = settingsJson.FfmpegPath ?? "ffmpeg";
    }

    private void Save() {
        string? parentDirectory = Path.GetDirectoryName(_filePath);
        if (parentDirectory.IsNotNullOrEmpty() && !Directory.Exists(parentDirectory)) {
            Directory.CreateDirectory(parentDirectory!);
        }

        var jsonOptions = new JsonSerializerOptions {
            WriteIndented = true
        };

        var settingsJson = new SettingsJson {
            Theme = AppTheme switch {
                Theme.Light => SettingsJsonValues.ThemeLight,
                Theme.Dark => SettingsJsonValues.ThemeDark,
                _ => throw new ArgumentException(nameof(AppTheme))
            },
            Language = AppLanguage switch {
                Language.English => SettingsJsonValues.LanguageEnglish,
                Language.Russian => SettingsJsonValues.LanguageRussian,
                _ => throw new ArgumentException(nameof(AppLanguage))
            },
            CheckForUpdates = CheckForUpdates,
            FfmpegPath = FfmpegPath
        };

        File.Create(_filePath).Use(it => JsonSerializer.Serialize(it, settingsJson, jsonOptions));
    }

    [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
    private class SettingsJson {

        [JsonPropertyName("theme")]
        public string? Theme { get; init; }
        
        [JsonPropertyName("language")]
        public string? Language { get; init; }
        
        [JsonPropertyName("check_for_updates")]
        public bool? CheckForUpdates { get; init; }
        
        [JsonPropertyName("ffmpeg_path")]
        public string? FfmpegPath { get; init; }
    }
    
    private static class SettingsJsonValues {
        public const string ThemeLight = "light";
        public const string ThemeDark = "dark";
        public const string LanguageEnglish = "en";
        public const string LanguageRussian = "ru";
    }
}