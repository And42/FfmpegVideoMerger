using System.Windows;
using System.Windows.Media;

namespace FfmpegVideoMerger.Logic; 

public static class UiElementUtils {

    public static T? GetParentOfType<T>(this DependencyObject self) where T : DependencyObject {
        DependencyObject current = self;
        while (true) {
            DependencyObject? parent = VisualTreeHelper.GetParent(current);
            if (parent == null) {
                return null;
            }
            if (parent is T result) {
                return result;
            }
            current = parent;
        }
    }
}