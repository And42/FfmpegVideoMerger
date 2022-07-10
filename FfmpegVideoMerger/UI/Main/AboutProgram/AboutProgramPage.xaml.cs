using System;
using System.Windows.Navigation;
using FfmpegVideoMerger.Logic;

namespace FfmpegVideoMerger.UI.Main.AboutProgram; 

public partial class AboutProgramPage {

    private AboutProgramViewModel ViewModel {
        get => DataContext as AboutProgramViewModel ?? throw new InvalidOperationException();
        init => DataContext = value;
    }
    
    public AboutProgramPage() {
        ViewModel = new AboutProgramViewModel();

        InitializeComponent();
    }

    private void BrowserHyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e) {
        ProcessUtils.OpenLinkInBrowser(e.Uri.AbsoluteUri);
        e.Handled = true;
    }

    private void DirectoryHyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e) {
        ProcessUtils.OpenDirectory(e.Uri.AbsoluteUri);
        e.Handled = true;
    }
}