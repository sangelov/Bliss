using System;
using System.Collections.Generic;
using System.Linq;
using Bliss.Core.Rgb;

namespace Bliss.Core.Grayscale
{
	public class GrayscaleImage : Image, IImage
	{
		public GrayscaleImage(GrayscalePixel[,] pixels)
		{
			if (pixels == null)
			{
				throw new ArgumentNullException("pixels");
			}

			this.pixels = new IPixel[pixels.GetLength(0), pixels.GetLength(1)];
			Array.Copy(pixels, this.pixels, pixels.GetLength(0) * pixels.GetLength(1));
		}

		public GrayscaleImage(RgbImage rgbImage)
		{
			if (rgbImage == null)
			{
				throw new ArgumentNullException("rgbImage", "RgbImage constructor can't be invoked with null");
			}

			this.pixels = new IPixel[rgbImage.Height, rgbImage.Width];
			for (int i = 0; i < rgbImage.Width; i++)
			{
				for (int j = 0; j < rgbImage.Height; j++)
				{
					RgbPixel pixel = (RgbPixel)rgbImage.Pixels[j, i];
					double value = (0.2125 * pixel.Red) + (0.7154 * pixel.Green) + (0.0721 * pixel.Blue);
					this.pixels[j, i] = new GrayscalePixel(Convert.ToByte(value));
				}
			}
		}

		public override void ApplyEqualizedHistogram(IHistogram histogram)
		{
			List<int> equalizedValues = new List<int>(histogram.EqualizedValues);
			
			for (int row = 0; row < histogram.Image.Height; row++)
			{
				for (int column = 0; column < histogram.Image.Width; column++)
				{
					int level = ((GrayscalePixel)this.pixels[row, column]).Level;
					this.pixels[row, column] = new GrayscalePixel((byte)equalizedValues[level]);
				}
			}
		}

		public override IEnumerable<IHistogram> GetHistograms()
		{
			IntHistogram histogram = new IntHistogram(this, "Grayscale");
			foreach (GrayscalePixel pixel in this)
			{
				histogram.Add(pixel.Level);
			}
			yield return histogram;
		}
	}
}