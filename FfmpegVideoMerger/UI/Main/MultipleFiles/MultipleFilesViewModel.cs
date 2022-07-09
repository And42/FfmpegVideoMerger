using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FfmpegVideoMerger.Logic;
using FfmpegVideoMerger.UI.Base;

namespace FfmpegVideoMerger.UI.Main.MultipleFiles; 

public class MultipleFilesViewModel : ViewModel {

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
    private string _videoFilesRegex = "e (?<number>\\d+)";

    public string AudioFilesRegex {
        get => _audioFilesRegex;
        set => SetProperty(ref _audioFilesRegex, value);
    }
    private string _audioFilesRegex = "e (?<number>\\d+)";
    
    private readonly ActionCommand _deleteVideoFileCommand;
    private readonly ActionCommand _deleteAudioFileCommand;

    public MultipleFilesViewModel() {
        _deleteVideoFileCommand = new ActionCommand(DeleteVideoFileCommandExecute, canExecute: _ => true);
        _deleteAudioFileCommand = new ActionCommand(DeleteAudioFileCommandExecute, canExecute: _ => true);
    }

    private void DeleteVideoFileCommandExecute(object? parameter) {
        var videoModel = parameter as SingleFileViewModel ?? throw new InvalidOperationException();

        VideoFiles = VideoFiles.Where(it => it != videoModel).ToList();
    }
    
    private void DeleteAudioFileCommandExecute(object? parameter) {
        var audioModel = parameter as SingleFileViewModel ?? throw new InvalidOperationException();

        AudioFiles = AudioFiles.Where(it => it != audioModel).ToList();
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
                ApplyFilesRegex(VideoFiles, VideoFilesRegex);
                break;
            case nameof(AudioFilesRegex):
            case nameof(AudioFiles):
                ApplyFilesRegex(AudioFiles, AudioFilesRegex);
                break;
        }
    }

    private static void ApplyFilesRegex(IEnumerable<SingleFileViewModel> items, string pattern) {
        var regex = new Regex(pattern);
        foreach (var item in items) {
            var match = regex.Match(item.FileName);
            item.RelationNumber = match.Groups.TryGetValue("number", out Group? group) ? group.Value : "error";
        }
    }
}