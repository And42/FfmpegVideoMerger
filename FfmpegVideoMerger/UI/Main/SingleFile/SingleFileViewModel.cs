using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FfmpegVideoMerger.Logic;
using FfmpegVideoMerger.UI.Base;
using Microsoft.Win32;

namespace FfmpegVideoMerger.UI.Main.SingleFile; 

public class SingleFileViewModel : ViewModel {

    private CancellationTokenSource? _activeProcessCancellation;
    
    public string SingleVideoFilePath {
        get => _singleVideoFilePath;
        set => SetProperty(ref _singleVideoFilePath, value);
    }

    private string _singleVideoFilePath =
        @"E:\Фильмы\Атака титанов\9. Shingeki no Kyojin TV-4 part 2\[SOFCJ-Raws] Shingeki no Kyojin - The Final Season Part 2 - 01 (WEBRip 1920x1080 x264 AAC).mkv";

    public string SingleAudioFilePath {
        get => _singleAudioFilePath;
        set => SetProperty(ref _singleAudioFilePath, value);
    }

    private string _singleAudioFilePath =
        @"E:\Фильмы\Атака титанов\9. Shingeki no Kyojin TV-4 part 2\Rus Dubs\Dub [StudioBand]\[SOFCJ-Raws] Shingeki no Kyojin - The Final Season Part 2 - 01 (WEBRip 1920x1080 x264 AAC).mka";

    public string SingleOutputFile {
        get => _singleOutputFile;
        set => SetProperty(ref _singleOutputFile, value);
    }

    private string _singleOutputFile =
        @"E:\Фильмы\Атака титанов\9. Shingeki no Kyojin TV-4 part 2\[SOFCJ-Raws] Shingeki no Kyojin - The Final Season Part 2 - 01 (WEBRip 1920x1080 x264 AAC)_output.mkv";

    public string SingleFileFfmpegCommand => FfmpegCommandGenerator.Generate(
        videoPath: SingleVideoFilePath,
        audioPaths: new List<string> { SingleAudioFilePath },
        outputPath: SingleOutputFile
    );

    public string SingleFileFfmpegOutput {
        get => _singleFileFfmpegOutput;
        set => SetProperty(ref _singleFileFfmpegOutput, value);
    }
    private string _singleFileFfmpegOutput = string.Empty;

    public bool IsProcessing => _activeProcessCancellation != null;

    public ActionCommand ChooseSingleVideoFileCommand { get; }
    public ActionCommand ChooseSingleAudioFileCommand { get; }
    public ActionCommand ChooseSingleOutputFileCommand { get; }
    public ActionCommand ProcessSingleFileCommand { get; }
    public ActionCommand CancelProcessCommand { get; }

    public SingleFileViewModel() {
        ChooseSingleVideoFileCommand = new ActionCommand(ChooseSingleVideoFileCommandExecute, () => !IsProcessing);
        ChooseSingleAudioFileCommand = new ActionCommand(ChooseSingleAudioFileExecute, () => !IsProcessing);
        ChooseSingleOutputFileCommand = new ActionCommand(ChooseSingleOutputFileCommandExecute, () => !IsProcessing);
        ProcessSingleFileCommand = new ActionCommand(ProcessSingleFileCommandExecute, () => !IsProcessing);
        CancelProcessCommand = new ActionCommand(CancelProcessCommandExecute, () => IsProcessing);
    }

    private void ChooseSingleVideoFileCommandExecute() {
        var openDialog = new OpenFileDialog();
        if (openDialog.ShowDialog() != true) {
            return;
        }

        SingleVideoFilePath = openDialog.FileName;
    }

    private void ChooseSingleAudioFileExecute() {
        var openDialog = new OpenFileDialog();
        if (openDialog.ShowDialog() != true) {
            return;
        }

        SingleAudioFilePath = openDialog.FileName;
    }

    private void ChooseSingleOutputFileCommandExecute() {
        var saveDialog = new SaveFileDialog();
        if (saveDialog.ShowDialog() != true) {
            return;
        }

        SingleOutputFile = saveDialog.FileName;
    }

    private async void ProcessSingleFileCommandExecute() {
        try {
            SingleFileFfmpegOutput = string.Empty;
            _activeProcessCancellation = new CancellationTokenSource();
            OnPropertyChanged(nameof(IsProcessing));

            await CommandExecutor.Execute(
                "ffmpeg",
                SingleFileFfmpegCommand,
                data => SingleFileFfmpegOutput += data + Environment.NewLine,
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
            case nameof(SingleVideoFilePath):
            case nameof(SingleAudioFilePath):
            case nameof(SingleOutputFile):
                base.OnPropertyChanged(nameof(SingleFileFfmpegCommand));
                break;
            case nameof(IsProcessing):
                ChooseSingleVideoFileCommand.RaiseCanExecuteChanged();
                ChooseSingleAudioFileCommand.RaiseCanExecuteChanged();
                ChooseSingleOutputFileCommand.RaiseCanExecuteChanged();
                ProcessSingleFileCommand.RaiseCanExecuteChanged();
                CancelProcessCommand.RaiseCanExecuteChanged();
                break;
        }
    }
}