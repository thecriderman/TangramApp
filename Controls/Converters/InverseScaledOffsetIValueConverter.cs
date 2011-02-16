using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace TangramApp1._35.Controls.Converters
{
    /// <summary>
    /// [scale]/x + [offset]
    /// </summary>
    public class InverseScaledOffsetIValueConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(InverseScaledOffsetIValueConverter), new PropertyMetadata(1.0));
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset", typeof(double), typeof(InverseScaledOffsetIValueConverter), new PropertyMetadata(0.0));

        public double Scale
        {
            set
            {
                SetValue(ScaleProperty, value);
            }
            get
            {
                return (double)GetValue(ScaleProperty);
            }
        }

        public double Offset
        {
            set
            {
                SetValue(OffsetProperty, value);
            }
            get
            {
                return (double)GetValue(OffsetProperty);
            }
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Int32)
            {
                //how stupid is it that i have to explicitly cast to an int before i can cast to a double
                return Scale / ((double)((int)value)) + Offset;
            }
            
            return Scale / ((double)value) + Offset; //TADA!
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("Cant let you do that.");
        }

        #endregion
    }
}
