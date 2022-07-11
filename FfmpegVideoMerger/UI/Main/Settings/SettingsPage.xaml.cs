using System;
using System.Windows.Input;

namespace FfmpegVideoMerger.UI.Main.Settings; 

public partial class SettingsPage {

    private SettingsViewModel ViewModel {
        get => DataContext as SettingsViewModel ?? throw new InvalidOperationException();
        init => DataContext = value;
    }
    
    public SettingsPage() {
        ViewModel = new SettingsViewModel();
        
        InitializeComponent();
    }

    private void ColorScheme_OnMouseUp(object sender, MouseButtonEventArgs e) {
        ViewModel.IsDark = !ViewModel.IsDark;
    }

    private void CheckForUpdates_OnMouseUp(object sender, MouseButtonEventArgs e) {
        ViewModel.CheckForUpdates = !ViewModel.CheckForUpdates;
    }
}