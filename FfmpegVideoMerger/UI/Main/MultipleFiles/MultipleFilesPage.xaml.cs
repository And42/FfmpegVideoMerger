using System;
using System.IO;
using System.Windows.Input;
using FfmpegVideoMerger.Logic;

namespace FfmpegVideoMerger.UI.Main.MultipleFiles; 

public partial class MultipleFilesPage {

    private MultipleFilesViewModel ViewModel {
        get => DataContext as MultipleFilesViewModel ?? throw new InvalidOperationException();
        init => DataContext = value;
    }

    public MultipleFilesPage() {
        ViewModel = new MultipleFilesViewModel();
        
        InitializeComponent();
        
        VideoDropTarget.EnableFileDrop(
            fileNameValidator: file => !ViewModel.IsProcessing && File.Exists(file),
            onFilesDropped: ViewModel.OnVideoFilesDropped
        );
        AudioDropTarget.EnableFileDrop(
            fileNameValidator: file => !ViewModel.IsProcessing && File.Exists(file),
            onFilesDropped: ViewModel.OnAudioFilesDropped
        );
        OutputDirectoryPathTextBox.EnableFileDrop(
            fileNameValidator: file => !ViewModel.IsProcessing && Directory.Exists(file),
            onFilesDropped: files => ViewModel.OutputDirectoryPath = files[0]
        );
        
        FfmpegOutputTextBox.ScrollToEndWhenGotText();
        
        ZeroPaddingDigitsTextBox.SelectAllOnFocus();
    }

    private void VideoDropTarget_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
        if (ViewModel.PickVideosCommand.CanExecute(null)) {
            ViewModel.PickVideosCommand.Execute(null);
        }
    }

    private void AudioDropTarget_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
        if (ViewModel.PickAudiosCommand.CanExecute(null)) {
            ViewModel.PickAudiosCommand.Execute(null);
        }
    }
}