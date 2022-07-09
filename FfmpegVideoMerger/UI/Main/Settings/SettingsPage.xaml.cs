using System;

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
}