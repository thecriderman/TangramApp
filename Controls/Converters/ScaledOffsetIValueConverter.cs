using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace TangramApp1._35.Controls.Converters
{
    /// <summary>
    /// [scale]x + [offset]
    /// </summary>
    public class ScaledOffsetIValueConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(ScaledOffsetIValueConverter), new PropertyMetadata(1.0));
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset", typeof(double), typeof(ScaledOffsetIValueConverter), new PropertyMetadata(0.0));

        public double Scale
        {
            get
            {
                return (double)GetValue(ScaleProperty);
            }
            set
            {
                SetValue(ScaleProperty, value);
            }
        }

        public double Offset
        {
            get
            {
                return (double)GetValue(OffsetProperty);
            }
            set
            {
                SetValue(OffsetProperty, value);
            }
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is String)
            {
                double parsed;
                double.TryParse((string)value,out parsed); //pass by reference
                return parsed * Scale + Offset; //TADA
            }
            //assume type is of number type
            return ((double)value) * Scale + Offset; //TADA
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("Cant let you do that.");
        }

        #endregion
    }
}
