using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bliss.Core;
using Bliss.Core.Grayscale;
using Bliss.Core.Hsl;

namespace Bliss.Wpf.Adapters
{
	public static class ImageAdapterFactory
	{
		public static ImageAdapter CreateAdapter(IImage image)
		{
			if (image is GrayscaleImage)
			{
				return new GrayscaleImageAdapter(image as GrayscaleImage);
			}
			else if (image is HslImage)
			{
				return new HslImageAdapter(image as HslImage);
			}
			return null;
		}

		public static ImageAdapter Create(string path)
		{
			BitmapSource source = new BitmapImage(new Uri(path));

			if (source.Format != PixelFormats.Bgra32)
			{
				source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
			}

			if (source.IsGrayscaleImage())
			{
				return new GrayscaleImageAdapter(source);
			}
			else
			{
				return new HslImageAdapter(path);
			}
		}
	}

	public static class Extensions
	{
		public static bool IsGrayscaleImage(this BitmapSource source)
		{
			if (source.Format != PixelFormats.Bgra32)
			{
				throw new InvalidOperationException("the source must be with Bgra32 pixel format");
			}

			int len = (int)source.Width * (int)source.Height * 4;
			byte[] pixels = new byte[len];
			source.CopyPixels(pixels, (int)source.Width * 4, 0);
			for (int i = 0; i < len; i = i + 4)
			{
				// if alpha is != 0 and  not r == g == b
				if (pixels[i + 3] != 0 && ((pixels[i] != pixels[i + 1] || pixels[i + 1] != pixels[i + 2])))
				{
					return false;
				}
			}
			return true;
		}
	}
}