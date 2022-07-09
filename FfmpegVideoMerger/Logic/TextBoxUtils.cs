using System;
using System.Windows.Controls;

namespace FfmpegVideoMerger.Logic; 

public static class TextBoxUtils {

    public static void ScrollToEndWhenGotText(this TextBox self) {
        self.TextChanged += (sender, _) => {
            var box = (TextBox) sender;
            var currentOffset = box.VerticalOffset + box.ViewportHeight;
            var totalOffset = box.ExtentHeight;
            if (Math.Abs(totalOffset - currentOffset) < 1) {
                box.ScrollToEnd();
            }
        };
    }

    public static void SelectAllOnFocus(this TextBox self) {
        self.GotFocus += (sender, _) => {
            var box = (TextBox) sender;
            box.Dispatcher.BeginInvoke(() => box.SelectAll());
        };
    }
}