using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using FfmpegVideoMerger.Resources.Localizations;
using MessageBoxResult = AdonisUI.Controls.MessageBoxResult;

namespace FfmpegVideoMerger.Logic.Versioning; 

public static class NewVersionChecker {

    public static void CheckInBackground() {
        Task.Run(CheckAsync);
    }

    private static async Task CheckAsync() {
        int[]? appVersion = GetAppVersion();
        if (appVersion == null) {
            return;
        }
        
        GithubVersionProvider.Data? latestVersion = await GithubVersionProvider.GetLatestVersionAsync(appVersion);
        if (latestVersion == null) {
            return;
        }

        for (int i = 0; i < Math.Max(appVersion.Length, latestVersion.Version.Length); ++i) {
            var appNumber = i < appVersion.Length ? appVersion[i] : 0;
            var latestNumber = i < latestVersion.Version.Length ? latestVersion.Version[i] : 0;
            if (latestNumber > appNumber) {
                ShowUpdateDialog(latestVersion);
                break;
            }
        }
    }

    private static void ShowUpdateDialog(GithubVersionProvider.Data data) {
        Application.Current.Dispatcher.Invoke(() => {
            string message = StringResources.NewVersionAvailable.Format(data.Version.JoinToString('.'));
            if (MessageBoxUtils.ShowInformationYesNo(message) == MessageBoxResult.Yes) {
                ProcessUtils.OpenLinkInBrowser(data.Url);
            }
        });
    }

    private static int[]? GetAppVersion() {
        Version? version = Assembly.GetExecutingAssembly().GetName().Version;
        if (version == null) {
            Trace.TraceWarning("Unable to get app version");
            return null;
        }

        return new[] { version.Major, version.Minor, version.Build, version.Revision };
    }
}