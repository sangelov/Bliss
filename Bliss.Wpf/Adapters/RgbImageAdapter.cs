using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bliss.Core.Rgb;

namespace Bliss.Wpf.Adapters
{
	public class RgbImageAdapter : ImageAdapter
	{
		public RgbImageAdapter(string path)
		{
			this.source = new BitmapImage(new Uri(path));

			if (source.Format != PixelFormats.Bgra32)
			{
				source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
			}

			int width = source.PixelWidth;
			int height = source.PixelHeight;
			RgbPixel[,] result = new RgbPixel[height, width];

			byte[] byteArray = new byte[height * width * 4];
			int stride = width * 4;
			source.CopyPixels(byteArray, stride, 0);
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					result[i, j] = new RgbPixel
					{
						Blue = byteArray[(i * width + j) * 4 + 0],
						Green = byteArray[(i * width + j) * 4 + 1],
						Red = byteArray[(i * width + j) * 4 + 2],
						Alpha = byteArray[(i * width + j) * 4 + 3],
					};
				}
			}

			image = new RgbImage(result);
		}
	}
}