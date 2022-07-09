using System;
using System.IO;
using System.Windows.Controls;
using FfmpegVideoMerger.Logic;

namespace FfmpegVideoMerger.UI.Main.MultipleFiles; 

public partial class MultipleFilesPage : Page {

    public MultipleFilesViewModel ViewModel {
        get => DataContext as MultipleFilesViewModel ?? throw new InvalidOperationException();
        init => DataContext = value;
    }
    
    public MultipleFilesPage() {
        ViewModel = new MultipleFilesViewModel();
        
        InitializeComponent();
        
        VideoDropTarget.EnableFileDrop(
            fileNameValidator: File.Exists,
            onFilesDropped: ViewModel.OnVideoFilesDropped
        );
        AudioDropTarget.EnableFileDrop(
            fileNameValidator: File.Exists,
            onFilesDropped: ViewModel.OnAudioFilesDropped
        );
    }
}