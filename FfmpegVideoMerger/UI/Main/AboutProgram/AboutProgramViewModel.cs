using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FfmpegVideoMerger.Logic;
using FfmpegVideoMerger.Logic.Storage;
using FfmpegVideoMerger.UI.Base;

namespace FfmpegVideoMerger.UI.Main.AboutProgram; 

public class AboutProgramViewModel : ViewModel {
    
    public string ExecutablePath { get; }
    public string DataPath { get; }
    public IReadOnlyList<LicenseViewModel> Licenses { get; }

    public AboutProgramViewModel() {
        Licenses = ParseLicenses();
        ExecutablePath = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
        DataPath = StoragePathsProvider.GetDataPath();
    }

    private static List<LicenseViewModel> ParseLicenses() {
        string licensesJson = ResourceUtils.GetAppResource("Resources.Licenses.json").Reader().Use(it => it.ReadToEnd());

        return JsonDocument.Parse(licensesJson).RootElement.EnumerateArray()
            .Select(licenseItem =>
                new LicenseViewModel(
                    Library: licenseItem.GetProperty("library").GetString() ?? throw new InvalidOperationException(),
                    Link: licenseItem.GetProperty("link").GetString() ?? throw new InvalidOperationException(),
                    Text: licenseItem.GetProperty("license").GetString() ?? throw new InvalidOperationException()
                )
            ).ToList();
    }
}