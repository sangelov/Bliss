using System;
using System.Collections.Generic;
using System.Linq;
using Bliss.Core.Rgb;

namespace Bliss.Core.Hsl
{
	public class HslImage : Image, IImage
	{
		public HslImage(HslPixel[,] pixels)
		{
			if (pixels == null)
			{
				throw new ArgumentNullException("pixels", "HslImage constructor can't be invoked with null");
			}

			this.pixels = new IPixel[pixels.GetLength(0), pixels.GetLength(1)];
			Array.Copy(pixels, this.pixels, pixels.GetLength(0) * pixels.GetLength(1));
		}

		public override IImage ApplyEqualizedHistogram(IHistogram histogram)
		{
			List<int> equalizedValues = new List<int>(histogram.EqualizedValues);
			HslPixel[,] pixels = new HslPixel[this.Height, this.Width];

			for (int row = 0; row < histogram.Image.Height; row++)
			{
				for (int column = 0; column < histogram.Image.Width; column++)
				{
					HslPixel pixel = ((HslPixel)this.pixels[row, column]);
					
					int oldValue = ((HslPixel)this.pixels[row, column]).NormalizedLuminance;
					pixels[row, column] = new HslPixel(pixel.Hue, pixel.Saturation, (float)(equalizedValues[oldValue] / 100.0));
				}
			}
			return new HslImage(pixels);
		}

		public override IEnumerable<IHistogram> GetHistograms()
		{
			IntHistogram histogram = new IntHistogram(this, "HSL Luminance channel", 101);
			foreach (HslPixel pixel in this)
			{
				histogram.Add(pixel.NormalizedLuminance);
			}

			yield return histogram;

			RgbImage rgbImage = this.ToRgbImage();
			foreach (IHistogram rgbHistogram in rgbImage.GetHistograms())
			{
				yield return rgbHistogram;
			}
		}
	}
}