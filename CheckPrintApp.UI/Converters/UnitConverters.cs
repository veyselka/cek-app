using System;
using System.Globalization;
using System.Windows.Data;
using CheckPrintApp.Core.Helpers;

namespace CheckPrintApp.UI.Converters;

/// <summary>
/// Milimetre değerini piksele çeviren converter
/// </summary>
public class MmToPixelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double mm)
        {
            // Base değeri al (parameter olarak geçilmiş)
            double baseValue = 0;
            if (parameter is string paramStr && double.TryParse(paramStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double paramValue))
            {
                baseValue = paramValue;
            }

            // Offset'i uygula ve piksele çevir
            double totalMm = baseValue + mm;
            return UnitConverter.MmToPixel(totalMm);
        }
        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Kalibrasyon offset'ini base koordinata ekleyip piksele çeviren converter
/// </summary>
public class CalibratedPositionConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length >= 2 && 
            values[0] is double basePosition && 
            values[1] is double offset)
        {
            // Ölçek faktörü (önizleme için küçültme)
            double scale = 3.0; // 1mm = 3 pixel (önizleme için)
            
            return (basePosition + offset) * scale;
        }
        return 0.0;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
