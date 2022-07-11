using AdonisUI.Controls;
using FfmpegVideoMerger.Resources.Localizations;

namespace FfmpegVideoMerger.Logic; 

public static class MessageBoxUtils {

    public static MessageBoxResult ShowInformationYesNo(string text) {
        MessageBoxModel messageBoxModel = new() {
            Text = text,
            Caption = StringResources.Information,
            Buttons = MessageBoxButtons.Create(MessageBoxButton.YesNo, StringResources.Yes, StringResources.No),
            Icon = MessageBoxImage.Information
        };
        return MessageBox.Show(messageBoxModel);
    }
    
    public static MessageBoxResult ShowError(string text) {
        return MessageBox.Show(
            caption: StringResources.Error,
            text: text,
            icon: MessageBoxImage.Warning
        );
    }
}