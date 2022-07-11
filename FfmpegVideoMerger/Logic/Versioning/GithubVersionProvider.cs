using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FfmpegVideoMerger.Resources.Localizations;

namespace FfmpegVideoMerger.Logic.Versioning; 

public static class GithubVersionProvider {

    private const string LatestApiReleaseUrl = "https://api.github.com/repos/And42/FfmpegVideoMerger/releases/latest";

    public record Data(int[] Version, string Url);
    
    public static async Task<Data?> GetLatestVersionAsync(
        int[] appVersion,
        CancellationToken cancellationToken = default
    ) {
        var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue(StringResources.AppName, appVersion.JoinToString('.'))
        );

        string resultJson;
        try {
            resultJson = await client.GetStringAsync(LatestApiReleaseUrl, cancellationToken);
        } catch (TaskCanceledException) {
            throw;
        } catch (Exception) {
            return null;
        }

        try {
            var document = JsonDocument.Parse(resultJson);
            string versionProperty = document.RootElement.GetProperty("name").GetString()
                ?? throw new Exception("No name property found");
            string urlProperty = document.RootElement.GetProperty("html_url").GetString()
                ?? throw new Exception("No html_url property found");
            
            var version = ParseVersion(versionProperty);

            return new Data(version, urlProperty);
        } catch (Exception ex) {
            Trace.TraceWarning("Unable to parse GitHub api output. Error: \"{0}\"", ex);
            return null;
        }
    }

    private static int[] ParseVersion(string version) {
        if (version[0] == 'v') {
            version = version[1..];
        }
        int dashIndex = version.IndexOf('-');
        if (dashIndex >= 0) {
            version = version[..dashIndex];
        }

        return version.Split('.').ConvertAll(int.Parse);
    }
}