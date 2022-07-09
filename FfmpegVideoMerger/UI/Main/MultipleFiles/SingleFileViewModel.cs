using System.IO;
using System.Windows.Input;
using FfmpegVideoMerger.UI.Base;

namespace FfmpegVideoMerger.UI.Main.MultipleFiles;

public class SingleFileViewModel : ViewModel {

    public string FilePath { get; }

    public string FileName { get; }

    public string RelationNumber {
        get => _relationNumber;
        set => SetProperty(ref _relationNumber, value);
    }
    private string _relationNumber = string.Empty;

    public ICommand DeleteCommand { get; }

    public SingleFileViewModel(
        string filePath,
        ICommand deleteCommand
    ) {
        FilePath = filePath;
        FileName = Path.GetFileName(filePath);
        DeleteCommand = deleteCommand;
    }
}