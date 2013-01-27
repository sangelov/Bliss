using System;
using System.Collections.Generic;
using System.Linq;
using Bliss.Core.Rgb;

namespace Bliss.Core.Grayscale
{
	public class GrayscaleImage : Image, IImage
	{
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

		public override IEnumerable<IHistogram> GetHistograms()
		{
			IntHistogram histogram = new IntHistogram("Grayscale");
			foreach (GrayscalePixel pixel in this)
			{
				histogram.Add(pixel.Level);
			}
			yield return histogram;
		}
	}
}