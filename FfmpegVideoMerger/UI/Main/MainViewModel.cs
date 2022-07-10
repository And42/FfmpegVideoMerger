using System;
using System.Collections.Generic;
using System.Windows;
using FfmpegVideoMerger.Resources.Localizations;
using FfmpegVideoMerger.UI.Base;
using FfmpegVideoMerger.UI.Main.AboutProgram;
using FfmpegVideoMerger.UI.Main.MultipleFiles;
using FfmpegVideoMerger.UI.Main.Settings;
using FfmpegVideoMerger.UI.Main.SingleFile;

namespace FfmpegVideoMerger.UI.Main; 

public class MainViewModel : ViewModel {

    public enum Page {
        SingleFile,
        MultipleFiles,
        Settings,
        AboutProgram
    }
    
    public IReadOnlyList<PageViewModel> Pages { get; }

    public UIElement CurrentPageContent => CurrentPage switch {
        Page.SingleFile => new SingleFilePage(),
        Page.MultipleFiles => new MultipleFilesPage(),
        Page.Settings => new SettingsPage(),
        Page.AboutProgram => new AboutProgramPage(),
        _ => throw new ArgumentOutOfRangeException(nameof(CurrentPage), CurrentPage, null)
    };

    public Page CurrentPage {
        get => _currentPage;
        private set => SetProperty(ref _currentPage, value);
    }
    private Page _currentPage = Page.SingleFile;

    public MainViewModel() {
        Pages = new PageViewModel[] {
            new(StringResources.SingleFile, Page.SingleFile),
            new(StringResources.MultipleFiles, Page.MultipleFiles),
            new(StringResources.Settings, Page.Settings),
            new(StringResources.AboutProgram, Page.AboutProgram)
        };
        AdjustPages();
    }
    
    public void GoToPage(Page page) {
        CurrentPage = page;
    }

    private void AdjustPages() {
        foreach (var page in Pages) {
            page.IsSelected = page.Page == CurrentPage;
        }
    }

    protected override void OnPropertyChanged(string? propertyName = null) {
        base.OnPropertyChanged(propertyName);

        switch (propertyName) {
            case nameof(CurrentPage):
                base.OnPropertyChanged(nameof(CurrentPageContent));
                AdjustPages();
                break;
        }
    }
}