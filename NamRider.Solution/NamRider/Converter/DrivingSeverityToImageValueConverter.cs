using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace NamRider.Converter
{
    class DrivingSeverityToImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value == null)
            {
                return new BitmapImage(new Uri("ms-appx:/Images/okayPin.png"));
            }
            var severity = (int)value;

            if (severity<25)
                return new BitmapImage(new Uri("ms-appx:/Images/okayPin.png"));
            if (severity<50)
                return new BitmapImage(new Uri("ms-appx:/Images/carePin.png"));
            if (severity<75)
                return new BitmapImage(new Uri("ms-appx:/Images/badPin.png"));
            return new BitmapImage(new Uri("ms-appx:/Images/awfulPin.png"));
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
