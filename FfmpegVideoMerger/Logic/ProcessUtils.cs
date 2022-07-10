using System;
using System.Diagnostics;
using AdonisUI.Controls;
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
            MessageBox.Show(
                caption: StringResources.Error,
                text: StringResources.UnableToOpenLink.Format(exception.Message),
                icon: MessageBoxImage.Warning
            );
        }
    }

    public static void OpenDirectory(string path) {
        var processInfo = new ProcessStartInfo(path) {
            UseShellExecute = true
        };

        try {
            Process.Start(processInfo);
        } catch (Exception exception) {
            MessageBox.Show(
                caption: StringResources.Error,
                text: StringResources.UnableToOpenDirectory.Format(exception.Message),
                icon: MessageBoxImage.Warning
            );
        }
    }
}