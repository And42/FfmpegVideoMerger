using System;
using System.IO;
using System.Windows.Input;
using FfmpegVideoMerger.Logic;

namespace FfmpegVideoMerger.UI.Main.Settings; 

public partial class SettingsPage {

    private SettingsViewModel ViewModel {
        get => DataContext as SettingsViewModel ?? throw new InvalidOperationException();
        init => DataContext = value;
    }
    
    public SettingsPage() {
        ViewModel = App.AppComponent.SettingsViewModel;
        
        InitializeComponent();
        
        FfmpegPathTextBox.EnableFileDrop(
            fileNameValidator: file => Path.GetExtension(file) == ".exe",
            onFilesDropped: files => ViewModel.FfmpegPath = files[0]
        );
    }

    private void ColorScheme_OnMouseUp(object sender, MouseButtonEventArgs e) {
        ViewModel.IsDark = !ViewModel.IsDark;
    }

    private void CheckForUpdates_OnMouseUp(object sender, MouseButtonEventArgs e) {
        ViewModel.CheckForUpdates = !ViewModel.CheckForUpdates;
    }
}