using System;

namespace FfmpegVideoMerger.UI.Main; 

public partial class MainWindow {

    private MainViewModel ViewModel {
        get => DataContext as MainViewModel ?? throw new InvalidOperationException();
        init => DataContext = value;
    }

    public MainWindow() {
        ViewModel = new MainViewModel();

        InitializeComponent();
    }
}