using System;
using System.Globalization;
using System.Windows.Data;

namespace FfmpegVideoMerger.Logic.Converters; 

public class CommandParameterEqualsConverter : IValueConverter {

    private static readonly object True = true;
    private static readonly object False = false;
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return Equals(value, parameter) ? True : False;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}