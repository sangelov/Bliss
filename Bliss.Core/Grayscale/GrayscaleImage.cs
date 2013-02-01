using System;
using System.Collections.Generic;
using System.Linq;

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

		public override IImage ApplyEqualizedHistogram(IHistogram histogram)
		{
			List<int> equalizedValues = new List<int>(histogram.EqualizedValues);
			GrayscalePixel[,] pixels = new GrayscalePixel[this.Height, this.Width];
			
			for (int row = 0; row < histogram.Image.Height; row++)
			{
				for (int column = 0; column < histogram.Image.Width; column++)
				{
					int level = ((GrayscalePixel)this.pixels[row, column]).Level;
					pixels[row, column] = new GrayscalePixel((byte)equalizedValues[level]);
				}
			}
			return new GrayscaleImage(pixels);
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