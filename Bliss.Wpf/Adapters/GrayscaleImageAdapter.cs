using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bliss.Core.Grayscale;

namespace Bliss.Wpf.Adapters
{
	public class GrayscaleImageAdapter : ImageAdapter
	{
		public GrayscaleImageAdapter(BitmapSource rgbSource)
		{
			if (rgbSource == null)
			{
				throw new ArgumentNullException("source");
			}

			int width = rgbSource.PixelWidth;
			int height = rgbSource.PixelHeight;
			GrayscalePixel[,] result = new GrayscalePixel[height, width];

			byte[] byteArray = new byte[height * width * 4];
			byte[] grayscaleArray = new byte[height * width];
			int stride = width * 4;
			rgbSource.CopyPixels(byteArray, stride, 0);
			int k = 0;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					byte blue = byteArray[(i * width + j) * 4 + 0];
					byte green = byteArray[(i * width + j) * 4 + 1];
					byte red = byteArray[(i * width + j) * 4 + 2];
					GrayscalePixel grayscalePixel = new GrayscalePixel(Convert.ToByte(0.2125 * red + 0.7154 * green + 0.0721 * blue));
					result[i, j] = grayscalePixel;
					grayscaleArray[k++] = grayscalePixel.Level;
				}
			}

			this.image = new GrayscaleImage(result);
			this.source = BitmapSource.Create(image.Width, image.Height, 96, 96, PixelFormats.Gray8, null, grayscaleArray, image.Width);
		}

		public GrayscaleImageAdapter(GrayscaleImage image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			this.image = image;
			byte[] src = image.Pixels.Cast<GrayscalePixel>().Select(x => x.Level).ToArray();
			this.source = BitmapSource.Create(image.Width, image.Height, 96, 96, PixelFormats.Gray8, null, src, image.Width);
		}
	}
}