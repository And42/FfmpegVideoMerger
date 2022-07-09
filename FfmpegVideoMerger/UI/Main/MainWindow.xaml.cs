using System;

namespace FfmpegVideoMerger.UI.Main; 

public partial class MainWindow {

    public static MainWindow? ActiveInstance { get; private set; }

    private MainViewModel ViewModel {
        get => DataContext as MainViewModel ?? throw new InvalidOperationException();
        init => DataContext = value;
    }

    public MainWindow() {
        ViewModel = new MainViewModel();

        InitializeComponent();

        ActiveInstance = this;
    }
}