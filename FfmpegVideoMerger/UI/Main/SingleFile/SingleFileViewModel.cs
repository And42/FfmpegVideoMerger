using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FfmpegVideoMerger.Logic;
using FfmpegVideoMerger.Logic.Settings;
using FfmpegVideoMerger.Resources.Localizations;
using FfmpegVideoMerger.UI.Base;
using Microsoft.Win32;

namespace FfmpegVideoMerger.UI.Main.SingleFile; 

public class SingleFileViewModel : ViewModel {

    private CancellationTokenSource? _activeProcessCancellation;
    
    public string VideoFilePath {
        get => _videoFilePath;
        set => SetProperty(ref _videoFilePath, value);
    }
    private string _videoFilePath = string.Empty;

    public string AudioFilePath {
        get => _audioFilePath;
        set => SetProperty(ref _audioFilePath, value);
    }
    private string _audioFilePath = string.Empty;

    public string OutputFile {
        get => _outputFile;
        set => SetProperty(ref _outputFile, value);
    }
    private string _outputFile = string.Empty;

    public string FfmpegCommand => FfmpegCommandGenerator.Generate(
        videoPath: VideoFilePath,
        audioPath: AudioFilePath,
        outputPath: OutputFile
    );

    public string FfmpegOutput {
        get => _ffmpegOutput;
        private set => SetProperty(ref _ffmpegOutput, value);
    }
    private string _ffmpegOutput = string.Empty;

    public bool IsProcessing => _activeProcessCancellation != null;

    public ActionCommand ChooseVideoFileCommand { get; }
    public ActionCommand ChooseAudioFileCommand { get; }
    public ActionCommand ChooseOutputFileCommand { get; }
    public ActionCommand ProcessFileCommand { get; }
    public ActionCommand CancelProcessCommand { get; }

    public SingleFileViewModel() {
        ChooseVideoFileCommand = new ActionCommand(ChooseVideoFileCommandExecute, () => !IsProcessing);
        ChooseAudioFileCommand = new ActionCommand(ChooseAudioFileExecute, () => !IsProcessing);
        ChooseOutputFileCommand = new ActionCommand(ChooseOutputFileCommandExecute, () => !IsProcessing);
        ProcessFileCommand = new ActionCommand(ProcessFileCommandExecute, () => !IsProcessing);
        CancelProcessCommand = new ActionCommand(CancelProcessCommandExecute, () => IsProcessing);
    }

    private void ChooseVideoFileCommandExecute() {
        var openDialog = new OpenFileDialog();
        if (openDialog.ShowDialog() != true) {
            return;
        }

        VideoFilePath = openDialog.FileName;
    }

    private void ChooseAudioFileExecute() {
        var openDialog = new OpenFileDialog();
        if (openDialog.ShowDialog() != true) {
            return;
        }

        AudioFilePath = openDialog.FileName;
    }

    private void ChooseOutputFileCommandExecute() {
        var saveDialog = new SaveFileDialog();
        if (saveDialog.ShowDialog() != true) {
            return;
        }

        OutputFile = saveDialog.FileName;
    }

    private async void ProcessFileCommandExecute() {
        string ffmpegPath = SettingsProvider.LoadSettings().FfmpegPath;
        if (!File.Exists(ffmpegPath)) {
            MessageBoxUtils.ShowError(StringResources.FfmpegNotFound.Format(ffmpegPath));
            return;
        }
        
        try {
            FfmpegOutput = string.Empty;
            _activeProcessCancellation = new CancellationTokenSource();
            OnPropertyChanged(nameof(IsProcessing));

            await CommandExecutor.Execute(
                ffmpegPath,
                FfmpegCommand,
                data => FfmpegOutput += data + Environment.NewLine,
                _activeProcessCancellation.Token
            );
        } catch (TaskCanceledException) {
        } finally {
            _activeProcessCancellation = null;
            OnPropertyChanged(nameof(IsProcessing));
        }
    }
    
    private void CancelProcessCommandExecute() {
        _activeProcessCancellation?.Cancel();
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        base.OnPropertyChanged(propertyName);
        
        switch (propertyName) {
            case nameof(VideoFilePath):
            case nameof(AudioFilePath):
            case nameof(OutputFile):
                base.OnPropertyChanged(nameof(FfmpegCommand));
                break;
            case nameof(IsProcessing):
                ChooseVideoFileCommand.RaiseCanExecuteChanged();
                ChooseAudioFileCommand.RaiseCanExecuteChanged();
                ChooseOutputFileCommand.RaiseCanExecuteChanged();
                ProcessFileCommand.RaiseCanExecuteChanged();
                CancelProcessCommand.RaiseCanExecuteChanged();
                break;
        }
    }
}