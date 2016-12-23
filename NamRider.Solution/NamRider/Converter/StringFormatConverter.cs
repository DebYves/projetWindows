using System;
using Windows.UI.Xaml.Data;

namespace NamRider.Converter
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //no value provided
            if (value == null)
            {
                return null;
            }
            //no format provided
            if (parameter == null)
            {
                return value;
            }
            return String.Format((String)parameter, value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}