using System.Windows;
using System.Windows.Input;

namespace FfmpegVideoMerger.Logic.AttachedProperties; 

public static class ScrollViewerConfigurator {
    
    public static readonly DependencyProperty DisableChildrenScrollProperty = DependencyProperty.RegisterAttached(
        "DisableChildrenScroll",
        typeof(bool),
    typeof(ScrollViewerConfigurator),
        new PropertyMetadata(false, OnDisableChildrenScrollChanged)
    );
    
    public static void SetDisableChildrenScroll(DependencyObject element, bool value)
    {
        element.SetValue(DisableChildrenScrollProperty, value);
    }

    public static bool GetDisableChildrenScroll(DependencyObject element)
    {
        return (bool)element.GetValue(DisableChildrenScrollProperty);
    }

    private static void OnDisableChildrenScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (true.Equals(e.NewValue)) {
            d.As<UIElement>().PreviewMouseWheel += DisableChildrenScroll_OnPreviewMouseWheel;
        } else {
            d.As<UIElement>().PreviewMouseWheel -= DisableChildrenScroll_OnPreviewMouseWheel;
        }
    }
    
    private static void DisableChildrenScroll_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e) {
        e.Handled = true;

        var newEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta) {
            RoutedEvent = UIElement.MouseWheelEvent
        };
        sender.As<UIElement>().RaiseEvent(newEventArgs);
    }
}