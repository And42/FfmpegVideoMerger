using System;
using System.Windows.Controls;
using FfmpegVideoMerger.Logic;
using FfmpegVideoMerger.UI.Base;

namespace FfmpegVideoMerger.UI.Main.SingleFile; 

public partial class SingleFilePage : Page {

    public SingleFileViewModel ViewModel {
        get => DataContext as SingleFileViewModel ?? throw new InvalidOperationException();
        init => DataContext = value;
    }

    public SingleFilePage() {
        ViewModel = new SingleFileViewModel();
        
        InitializeComponent();
        
        SingleVideoFileTextBox.EnableFileDrop(
            fileNameValidator: _ => !ViewModel.IsProcessing,
            onFilesDropped: files => ViewModel.SingleVideoFilePath = files[0]
        );
        SingleAudioFileTextBox.EnableFileDrop(
            fileNameValidator: _ => !ViewModel.IsProcessing,
            onFilesDropped: files => ViewModel.SingleAudioFilePath = files[0]
        );
        SingleOutputFileTextBox.EnableFileDrop(
            fileNameValidator: _ => !ViewModel.IsProcessing,
            onFilesDropped: files => ViewModel.SingleOutputFile = files[0]
        );

        SingleFileFfmpegOutputTextBox.TextChanged += (_, _) => {
            var box = SingleFileFfmpegOutputTextBox;
            var currentOffset = box.VerticalOffset + box.ViewportHeight;
            var totalOffset = box.ExtentHeight;
            if (Math.Abs(totalOffset - currentOffset) < 1) {
                box.ScrollToEnd();
            }
        };
    }
}