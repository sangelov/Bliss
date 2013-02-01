using System;
using System.Collections.Generic;
using System.Linq;

namespace Bliss.Core.Rgb
{
	public class RgbImage : Image, IImage
	{
		public RgbImage(RgbPixel[,] pixels)
		{
			if (pixels == null)
			{
				throw new ArgumentNullException("pixels", "RgbImage constructor can't be invoked with null");
			}

			this.pixels = new IPixel[pixels.GetLength(0), pixels.GetLength(1)];
			Array.Copy(pixels, this.pixels, pixels.GetLength(0) * pixels.GetLength(1));
		}

		public override IImage ApplyEqualizedHistogram(IHistogram histogram)
		{
			// TODO: Implement this method
			throw new NotImplementedException();
		}

		public override IEnumerable<IHistogram> GetHistograms()
		{
			IntHistogram redHistogram = new IntHistogram(this, "Red");
			IntHistogram greenHistogram = new IntHistogram(this, "Green");
			IntHistogram blueHistogram = new IntHistogram(this, "Blue");

			foreach (RgbPixel pixel in this)
			{
				redHistogram.Add(pixel.Red);
				greenHistogram.Add(pixel.Green);
				blueHistogram.Add(pixel.Blue);
			}

			yield return redHistogram;
			yield return greenHistogram;
			yield return blueHistogram;
		}
	}
}