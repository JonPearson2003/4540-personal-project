using System.Globalization;

namespace BuildTrack.App.Converters;

public sealed class ByteArrayToImageSourceConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not byte[] imageBytes || imageBytes.Length == 0)
			return null;

		return ImageSource.FromStream(() => new MemoryStream(imageBytes, writable: false));
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		throw new NotSupportedException();
}
