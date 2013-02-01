using System;
using System.Linq;
using Bliss.Core.Grayscale;
using Bliss.Core.Hsl;
using Bliss.Core.Rgb;

namespace Bliss.Core
{
	public static class PixelConverter
	{
		public static RgbImage ToRgbImage(this HslImage hslImage)
		{
			RgbPixel[,] rgbPixels = new RgbPixel[hslImage.Height, hslImage.Width];

			for (int i = 0; i < hslImage.Height; i++)
			{
				for (int j = 0; j < hslImage.Width; j++)
				{
					HslPixel hslPixel = (HslPixel)hslImage.Pixels[i, j];
					rgbPixels[i, j] = hslPixel.ToRgbPixel();
				}
			}
			return new RgbImage(rgbPixels);
		}
		
		public static GrayscaleImage ToGrayscaleImage(this RgbImage rgbImage)
		{
			if (rgbImage == null)
			{
				throw new ArgumentNullException("rgbImage", "RgbImage constructor can't be invoked with null");
			}

			GrayscalePixel[,] grayscalePixels = new GrayscalePixel[rgbImage.Height, rgbImage.Width];
			for (int i = 0; i < rgbImage.Height; i++)
			{
				for (int j = 0; j < rgbImage.Width; j++)
				{
					RgbPixel rgbPixel = (RgbPixel)rgbImage.Pixels[i, j];
					grayscalePixels[i, j] = rgbPixel.ToGrayscalePixel();
				}
			}
			return new GrayscaleImage(grayscalePixels);
		}

		public static GrayscalePixel ToGrayscalePixel(this RgbPixel rgbPixel)
		{
			double grayLevel = (0.2125 * rgbPixel.Red) + (0.7154 * rgbPixel.Green) + (0.0721 * rgbPixel.Blue);
			return new GrayscalePixel(Convert.ToByte(grayLevel));
		}

		public static HslPixel CreateHslPixelFromRgb(Byte r, Byte g, Byte b)
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

		public static RgbPixel ToRgbPixel(this HslPixel pixel)
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

		private static double ColorCalc(double c, double t1, double t2)
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