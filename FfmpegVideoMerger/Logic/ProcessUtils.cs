using System;
using System.Diagnostics;
using FfmpegVideoMerger.Resources.Localizations;

namespace FfmpegVideoMerger.Logic; 

public static class ProcessUtils {

    public static void OpenLinkInBrowser(string link) {
        var processInfo = new ProcessStartInfo(link) {
            UseShellExecute = true
        };

        try {
            Process.Start(processInfo);
        } catch (Exception exception) {
            MessageBoxUtils.ShowError(StringResources.UnableToOpenLink.Format(exception.Message));
        }
    }

    public static void OpenDirectory(string path) {
        var processInfo = new ProcessStartInfo(path) {
            UseShellExecute = true
        };

        try {
            Process.Start(processInfo);
        } catch (Exception exception) {
            MessageBoxUtils.ShowError(StringResources.UnableToOpenDirectory.Format(exception.Message));
        }
    }
}