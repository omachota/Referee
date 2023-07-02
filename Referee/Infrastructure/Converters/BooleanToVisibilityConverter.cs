using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Referee.Infrastructure.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			return (bool)value ? Visibility.Hidden : Visibility.Visible;
		}

		return Visibility.Hidden;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
