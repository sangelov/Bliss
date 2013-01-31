using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bliss.Core.Hsl;
using Bliss.Core.Rgb;

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
				var color = this.CreateRgbPixelFromHsl(pixel);
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
					result[i, j] = CreateHslPixelFromRgb(red, green, blue);
				}
			}

			image = new HslImage(result);
		}

		private HslPixel CreateHslPixelFromRgb(Byte r, Byte g, Byte b)
		{
			float red = (r / 255f);
			float green = (g / 255f);
			float blue = (b / 255f);

			float min = Math.Min(Math.Min(red, green), blue);
			float max = Math.Max(Math.Max(red, green), blue);
			float delta = max - min;

			float hue = 0;
			float saturation = 0;
			float luminance = ((max + min) / 2.0f);

			if (delta != 0)
			{
				if (luminance < 0.5f)
				{
					saturation = (delta / (max + min));
				}
				else
				{
					saturation = (delta / (2.0f - max - min));
				}

				if (red == max)
				{
					hue = (green - blue) / delta;
				}
				else if (green == max)
				{
					hue = 2f + (blue - red) / delta;
				}
				else if (blue == max)
				{
					hue = 4f + (red - green) / delta;
				}
			}

			return new HslPixel(hue, saturation, luminance);
		}

		public RgbPixel CreateRgbPixelFromHsl(HslPixel pixel)
		{
			byte r, g, b;
			if (pixel.Saturation == 0)
			{
				r = (byte)Math.Round(pixel.Luminance * 255d);
				g = (byte)Math.Round(pixel.Luminance * 255d);
				b = (byte)Math.Round(pixel.Luminance * 255d);
			}
			else
			{
				double t1, t2;
				double th = pixel.Hue / 6.0d;

				if (pixel.Luminance < 0.5d)
				{
					t2 = pixel.Luminance * (1d + pixel.Saturation);
				}
				else
				{
					t2 = (pixel.Luminance + pixel.Saturation) - (pixel.Luminance * pixel.Saturation);
				}
				t1 = 2d * pixel.Luminance - t2;

				double tr, tg, tb;
				tr = th + (1.0d / 3.0d);
				tg = th;
				tb = th - (1.0d / 3.0d);

				tr = ColorCalc(tr, t1, t2);
				tg = ColorCalc(tg, t1, t2);
				tb = ColorCalc(tb, t1, t2);
				r = (byte)Math.Round(tr * 255d);
				g = (byte)Math.Round(tg * 255d);
				b = (byte)Math.Round(tb * 255d);
			}
			return new RgbPixel(r, g, b);
		}

		private double ColorCalc(double c, double t1, double t2)
		{
			if (c < 0)
			{
				c += 1d;
			}
			if (c > 1)
			{
				c -= 1d;
			}
			if (6.0d * c < 1.0d)
			{
				return t1 + (t2 - t1) * 6.0d * c;
			}
			if (2.0d * c < 1.0d)
			{
				return t2;
			}
			if (3.0d * c < 2.0d)
			{
				return t1 + (t2 - t1) * (2.0d / 3.0d - c) * 6.0d;
			}
			return t1;
		}
	}
}