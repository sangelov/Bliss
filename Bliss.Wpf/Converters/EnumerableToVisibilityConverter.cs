using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Bliss.Wpf.Converters
{
	public class EnumerableToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is IEnumerable)
			{
				IEnumerable enumerable = value as IEnumerable;
				foreach (var i in enumerable)
				{
					return Visibility.Visible;
				}
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// TODO: Implement this method
			throw new NotImplementedException();
		}
	}
}
