using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bliss.Core;
using Bliss.Core.Hsl;

namespace Bliss.Wpf.Adapters
{
	public class HslImageAdapter : ImageAdapter
	{
		public HslImageAdapter(HslImage image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			this.image = image;
			byte[] src = new byte[image.Width * image.Height * 4];
			int i = 0;
			foreach (HslPixel pixel in image)
			{
				var color = pixel.ToRgbPixel();
				src[i++] = color.Blue;
				src[i++] = color.Green;
				src[i++] = color.Red;
				src[i++] = color.Alpha;
			}
			this.source = BitmapSource.Create(image.Width, image.Height, 96, 96, PixelFormats.Pbgra32, null, src, image.Width * 4);
		}

		public HslImageAdapter(string path)
		{
			this.source = new BitmapImage(new Uri(path));

			if (source.Format != PixelFormats.Bgra32)
			{
				source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
			}

			int width = source.PixelWidth;
			int height = source.PixelHeight;
			HslPixel[,] result = new HslPixel[height, width];

			byte[] byteArray = new byte[height * width * 4];
			int stride = width * 4;
			source.CopyPixels(byteArray, stride, 0);
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					var blue = byteArray[(i * width + j) * 4 + 0];
					var green = byteArray[(i * width + j) * 4 + 1];
					var red = byteArray[(i * width + j) * 4 + 2];
					result[i, j] = PixelConverter.CreateHslPixelFromRgb(red, green, blue);
				}
			}

			image = new HslImage(result);
		}
	}
}