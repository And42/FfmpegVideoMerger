using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Brushes = AdonisUI.Brushes;

namespace FfmpegVideoMerger.Logic.Converters; 

public class IfTrueReturnAccentBrush : IValueConverter {

    private static readonly SolidColorBrush TransparentBrush = new(Colors.Transparent);
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return true.Equals(value) ? Brushes.AccentBrush : TransparentBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}