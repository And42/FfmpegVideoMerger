using System;
using FfmpegVideoMerger.Logic;

namespace FfmpegVideoMerger.UI.Main.SingleFile; 

public partial class SingleFilePage {

    private SingleFileViewModel ViewModel {
        get => DataContext as SingleFileViewModel ?? throw new InvalidOperationException();
        init => DataContext = value;
    }

    public SingleFilePage() {
        ViewModel = new SingleFileViewModel();
        
        InitializeComponent();
        
        VideoFileTextBox.EnableFileDrop(
            fileNameValidator: _ => !ViewModel.IsProcessing,
            onFilesDropped: files => ViewModel.VideoFilePath = files[0]
        );
        AudioFileTextBox.EnableFileDrop(
            fileNameValidator: _ => !ViewModel.IsProcessing,
            onFilesDropped: files => ViewModel.AudioFilePath = files[0]
        );
        OutputFileTextBox.EnableFileDrop(
            fileNameValidator: _ => !ViewModel.IsProcessing,
            onFilesDropped: files => ViewModel.OutputFile = files[0]
        );

        FfmpegOutputTextBox.ScrollToEndWhenGotText();
    }
}