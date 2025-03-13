using System.IO;
using System.Windows.Input;
using FfmpegVideoMerger.UI.Base;

namespace FfmpegVideoMerger.UI.Main.MultipleFiles;

public class SingleFileViewModel : ViewModel {

    public string FilePath { get; }

    public string FileName { get; }

    public string RelationId {
        get => _relationId;
        set => SetProperty(ref _relationId, value);
    }
    private string _relationId = string.Empty;

    public bool AutoModeForRelationId {
        get => _autoModeForRelationId;
        set => SetProperty(ref _autoModeForRelationId, value);
    }
    private bool _autoModeForRelationId = true;

    public ICommand DeleteCommand { get; }

    public SingleFileViewModel(
        string filePath,
        ICommand deleteCommand
    )
    {
        FilePath = filePath;
        FileName = Path.GetFileName(filePath);
        DeleteCommand = deleteCommand;
    }
}