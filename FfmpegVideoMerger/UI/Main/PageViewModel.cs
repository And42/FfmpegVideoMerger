using FfmpegVideoMerger.UI.Base;

namespace FfmpegVideoMerger.UI.Main;

public class PageViewModel : ViewModel {

    public string Title { get; }
    public MainViewModel.Page Page { get; }

    public bool IsSelected {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
    private bool _isSelected;
    
    public PageViewModel(string title, MainViewModel.Page page) {
        Title = title;
        Page = page;
    }
}