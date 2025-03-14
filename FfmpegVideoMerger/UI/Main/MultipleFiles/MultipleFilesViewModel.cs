﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdonisUI.Controls;
using FfmpegVideoMerger.Logic;
using FfmpegVideoMerger.Logic.Settings;
using FfmpegVideoMerger.Resources.Localizations;
using FfmpegVideoMerger.UI.Base;
using MessageBox = AdonisUI.Controls.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace FfmpegVideoMerger.UI.Main.MultipleFiles; 

public class MultipleFilesViewModel : ViewModel {

    private const string GroupKey = "id";

    private readonly AppSettings _appSettings;

    private CancellationTokenSource? _activeProcessCancellation;

    public bool IsProcessing => _activeProcessCancellation != null;
    
    public IReadOnlyList<SingleFileViewModel> VideoFiles {
        get => _videoFiles;
        private set => SetProperty(ref _videoFiles, value);
    }
    private IReadOnlyList<SingleFileViewModel> _videoFiles = Array.Empty<SingleFileViewModel>();
    
    public IReadOnlyList<SingleFileViewModel> AudioFiles {
        get => _audioFiles;
        private set => SetProperty(ref _audioFiles, value);
    }
    private IReadOnlyList<SingleFileViewModel> _audioFiles = Array.Empty<SingleFileViewModel>();

    public string VideoFilesRegex {
        get => _videoFilesRegex;
        set => SetProperty(ref _videoFilesRegex, value);
    }
    private string _videoFilesRegex = $"- (?<{GroupKey}>\\d+)";

    public string AudioFilesRegex {
        get => _audioFilesRegex;
        set => SetProperty(ref _audioFilesRegex, value);
    }
    private string _audioFilesRegex = $"- (?<{GroupKey}>\\d+)";

    public string OutputsExpression {
        get => _outputsExpression;
        set => SetProperty(ref _outputsExpression, value);
    }
    private string _outputsExpression = $"Episode <{GroupKey}>.mkv";

    public int ZeroPaddingDigits {
        get => _zeroPaddingDigits;
        set {
            if (value is >= 0 and <= 20) {
                SetProperty(ref _zeroPaddingDigits, value);
            }
        }
    }
    private int _zeroPaddingDigits = 1;

    public string ExampleOutputFileName => ApplyOutputExpression(id: "3");

    public string OutputDirectoryPath {
        get => _outputDirectoryPath;
        set => SetProperty(ref _outputDirectoryPath, value);
    }
    private string _outputDirectoryPath = string.Empty;
    
    public string FfmpegOutput {
        get => _ffmpegOutput;
        private set => SetProperty(ref _ffmpegOutput, value);
    }
    private string _ffmpegOutput = string.Empty;
    
    public double Progress {
        get => _progress;
        private set => SetProperty(ref _progress, value);
    }
    private double _progress;

    public bool VideoIdManualProcessingEnabled {
        get => _videoIdManualProcessingEnabled;
        set => SetProperty(ref _videoIdManualProcessingEnabled, value);
    }
    private bool _videoIdManualProcessingEnabled;
    
    public bool AudioIdManualProcessingEnabled {
        get => _audioIdManualProcessingEnabled;
        set => SetProperty(ref _audioIdManualProcessingEnabled, value);
    }
    private bool _audioIdManualProcessingEnabled;

    public ActionCommand PickVideosCommand { get; }
    public ActionCommand PickAudiosCommand { get; }
    public ActionCommand PickOutputDirectoryCommand { get; }
    public ActionCommand ProcessCommand { get; }
    public ActionCommand CancelCommand { get; }

    private readonly ActionCommand _deleteVideoFileCommand;
    private readonly ActionCommand _deleteAudioFileCommand;

    public MultipleFilesViewModel(
        AppSettings appSettings
    ) {
        _appSettings = appSettings;
        _deleteVideoFileCommand = new ActionCommand(DeleteVideoFileCommandExecute, canExecute: _ => !IsProcessing);
        _deleteAudioFileCommand = new ActionCommand(DeleteAudioFileCommandExecute, canExecute: _ => !IsProcessing);
        PickVideosCommand = new ActionCommand(PickVideosCommandExecute, canExecute: () => !IsProcessing);
        PickAudiosCommand = new ActionCommand(PickAudiosCommandExecute, canExecute: () => !IsProcessing);
        PickOutputDirectoryCommand =
            new ActionCommand(PickOutputDirectoryCommandExecute, canExecute: () => !IsProcessing);
        ProcessCommand = new ActionCommand(ProcessCommandExecute, canExecute: () => !IsProcessing);
        CancelCommand = new ActionCommand(CancelCommandExecute, canExecute: () => IsProcessing);
    }

    private void DeleteVideoFileCommandExecute(object? parameter) {
        var videoModel = parameter as SingleFileViewModel ?? throw new InvalidOperationException();

        VideoFiles = VideoFiles.Where(it => it != videoModel).ToList();
    }
    
    private void DeleteAudioFileCommandExecute(object? parameter) {
        var audioModel = parameter as SingleFileViewModel ?? throw new InvalidOperationException();

        AudioFiles = AudioFiles.Where(it => it != audioModel).ToList();
    }

    private void PickVideosCommandExecute() {
        var openDialog = new OpenFileDialog {
            Multiselect = true
        };
        if (openDialog.ShowDialog() != true) {
            return;
        }
        
        OnVideoFilesDropped(openDialog.FileNames);
    }
    
    private void PickAudiosCommandExecute() {
        var openDialog = new OpenFileDialog {
            Multiselect = true
        };
        if (openDialog.ShowDialog() != true) {
            return;
        }
        
        OnAudioFilesDropped(openDialog.FileNames);
    }
    
    private void PickOutputDirectoryCommandExecute() {
        var saveDialog = new FolderBrowserDialog();
        if (saveDialog.ShowDialog() != DialogResult.OK) {
            return;
        }

        OutputDirectoryPath = saveDialog.SelectedPath;
    }

    private async void ProcessCommandExecute() {
        void ShowErrorMessageBox(string text) {
            MessageBox.Show(
                text: text,
                caption: StringResources.Error,
                icon: MessageBoxImage.Warning
            );
        }
        
        var videoToName = VideoFiles.Select(video =>
            video.FilePath.To(ApplyOutputExpression(video.RelationId))
        ).ToList();

        if (videoToName.Select(it => it.Value).Distinct().Count() != videoToName.Count) {
            ShowErrorMessageBox(StringResources.VideoIdsNotUnique);
            return;
        }

        var audioToName = AudioFiles.Select(audio =>
            audio.FilePath.To(ApplyOutputExpression(audio.RelationId))
        ).ToList();
        
        if (audioToName.Select(it => it.Value).Distinct().Count() != audioToName.Count) {
            ShowErrorMessageBox(StringResources.AudioIdsNotUnique);
            return;
        }

        var filesToProcess = new List<FileToProcess>();
        
        foreach (var (videoPath, outputName) in videoToName) {
            int audioIndex = audioToName.FindIndex(it => it.Value == outputName);
            if (audioIndex < 0) {
                ShowErrorMessageBox(StringResources.NoAudioFoundForVideo.Format(videoPath, outputName));
                return;
            }
            
            filesToProcess.Add(
                new FileToProcess(
                    VideoPath: videoPath,
                    AudioPath: audioToName[audioIndex].Key,
                    OutputName: outputName
                )
            );
            
            audioToName.RemoveAt(audioIndex);
        }

        if (audioToName.IsNotEmpty()) {
            var audio = audioToName[0];
            ShowErrorMessageBox(StringResources.NoVideoFoundForAudio.Format(audio.Key, audio.Value));
            return;
        }
        
        string ffmpegPath = _appSettings.FfmpegPath;

        try {
            FfmpegOutput = string.Empty;
            _activeProcessCancellation = new CancellationTokenSource();
            OnPropertyChanged(nameof(IsProcessing));

            Progress = 0;
            foreach (var (index, fileToProcess) in filesToProcess.Indexed()) {
                string ffmpegCommand = FfmpegCommandGenerator.Generate(
                    videoPath: fileToProcess.VideoPath,
                    audioPath: fileToProcess.AudioPath,
                    outputPath: Path.Combine(OutputDirectoryPath, fileToProcess.OutputName)
                );

                await CommandExecutor.Execute(
                    ffmpegPath,
                    ffmpegCommand,
                    data => FfmpegOutput += data + Environment.NewLine,
                    _activeProcessCancellation.Token
                );

                Progress = (double)(index + 1) / filesToProcess.Count;
            }
        } catch (TaskCanceledException) {
        } catch (Win32Exception) {
            MessageBoxUtils.ShowError(StringResources.FfmpegNotFound.Format(ffmpegPath));
        }finally {
            _activeProcessCancellation = null;
            OnPropertyChanged(nameof(IsProcessing));
        }
    }

    private void CancelCommandExecute() {
        _activeProcessCancellation?.Cancel();
    }

    public void OnVideoFilesDropped(string[] files) {
        VideoFiles = files.ConvertAll(file =>
            new SingleFileViewModel(
                filePath: file,
                deleteCommand: _deleteVideoFileCommand
            )
        );
    }

    public void OnAudioFilesDropped(string[] files) {
        AudioFiles = files.ConvertAll(file =>
            new SingleFileViewModel(
                filePath: file,
                deleteCommand: _deleteAudioFileCommand
            )
        );
    }

    protected override void OnPropertyChanged(string? propertyName = null) {
        base.OnPropertyChanged(propertyName);

        switch (propertyName) {
            case nameof(VideoFilesRegex):
            case nameof(VideoFiles):
            case nameof(VideoIdManualProcessingEnabled):
                UpdateVideoModels();
                break;
            case nameof(AudioFilesRegex):
            case nameof(AudioFiles):
            case nameof(AudioIdManualProcessingEnabled):
                UpdateAudioModels();
                break;
            case nameof(OutputsExpression):
            case nameof(ZeroPaddingDigits):
                base.OnPropertyChanged(nameof(ExampleOutputFileName));
                break;
            case nameof(IsProcessing):
                _deleteVideoFileCommand.RaiseCanExecuteChanged();
                _deleteAudioFileCommand.RaiseCanExecuteChanged();
                PickVideosCommand.RaiseCanExecuteChanged();
                PickAudiosCommand.RaiseCanExecuteChanged();
                PickOutputDirectoryCommand.RaiseCanExecuteChanged();
                ProcessCommand.RaiseCanExecuteChanged();
                CancelCommand.RaiseCanExecuteChanged();
                break;
        }
    }

    private string ApplyOutputExpression(string id) {
        return OutputsExpression.Replace(
            oldValue: $"<{GroupKey}>",
            newValue: id.PadLeft(ZeroPaddingDigits, '0'),
            StringComparison.Ordinal
        );
    }

    private void UpdateVideoModels() {
        ApplyFilesRelationIdsMode(VideoFiles, VideoIdManualProcessingEnabled);
        if (!VideoIdManualProcessingEnabled) {
            ApplyFilesRegex(VideoFiles, VideoFilesRegex);
        }
    }
    
    private void UpdateAudioModels() {
        ApplyFilesRelationIdsMode(AudioFiles, AudioIdManualProcessingEnabled);
        if (!AudioIdManualProcessingEnabled) {
            ApplyFilesRegex(AudioFiles, AudioFilesRegex);
        }
    }
    
    private static void ApplyFilesRelationIdsMode(IEnumerable<SingleFileViewModel> items, bool manualProcessingEnabled) {
        foreach (SingleFileViewModel item in items) {
            item.AutoModeForRelationId = !manualProcessingEnabled;
        }
    }
    
    private static void ApplyFilesRegex(IEnumerable<SingleFileViewModel> items, string pattern) {
        var regex = new Regex(pattern);
        foreach (var item in items) {
            var match = regex.Match(item.FileName);
            item.RelationId = match.Groups.TryGetValue(GroupKey, out Group? group) ? group.Value : "error";
        }
    }

    private record FileToProcess(
        string VideoPath,
        string AudioPath,
        string OutputName
    );
}