using System;
using System.Globalization;
using FfmpegVideoMerger.Logic.Settings;

namespace FfmpegVideoMerger.Logic.Language; 

public class LanguageUtils {

    public static void SetLanguage(AppSettings.Language language) {
        var languageCode = language switch {
            AppSettings.Language.English => "en",
            AppSettings.Language.Russian => "ru",
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };

        var cultureInfo = CultureInfo.GetCultureInfo(languageCode);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }
}