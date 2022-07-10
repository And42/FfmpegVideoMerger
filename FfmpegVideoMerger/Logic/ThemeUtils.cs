using System;
using System.Windows;
using AdonisUI;
using FfmpegVideoMerger.Logic.Settings;

namespace FfmpegVideoMerger.Logic; 

public static class ThemeUtils {

    public static void SetTheme(AppSettings.Theme theme) {
        var themeUri = theme switch {
            AppSettings.Theme.Light => ResourceLocator.LightColorScheme,
            AppSettings.Theme.Dark => ResourceLocator.DarkColorScheme,
            _ => throw new ArgumentOutOfRangeException(nameof(theme), theme, null)
        };
        ResourceLocator.SetColorScheme(Application.Current.Resources, themeUri);
    }
}