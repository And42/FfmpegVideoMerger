using System;
using System.Windows;
using System.Windows.Input;
using FfmpegVideoMerger.Logic;

namespace FfmpegVideoMerger.UI.Main; 

public partial class MainWindow {

    public static MainWindow? ActiveInstance { get; private set; }

    public MainViewModel ViewModel {
        get => DataContext as MainViewModel ?? throw new InvalidOperationException();
        init => DataContext = value;
    }

    public MainWindow() {
        ViewModel = new MainViewModel();

        InitializeComponent();

        ActiveInstance = this;
    }

    private void PageTab_OnMouseUp(object sender, MouseButtonEventArgs e) {
        var page = sender.As<FrameworkElement>().DataContext.As<PageViewModel>().Page;
        ViewModel.GoToPage(page);
    }
}