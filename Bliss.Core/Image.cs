using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bliss.Core
{
	public abstract class Image : IImage
	{
		protected IPixel[,] pixels;

		public abstract void ApplyEqualizedHistogram(IHistogram histogram);

		public abstract IEnumerable<IHistogram> GetHistograms();

		public IEnumerator GetEnumerator()
		{
			return pixels.GetEnumerator();
		}

		IEnumerator<IPixel> IEnumerable<IPixel>.GetEnumerator()
		{
			return pixels.Cast<IPixel>().GetEnumerator();
		}

		public IPixel[,] Pixels
		{
			get
			{
				return this.pixels;
			}
		}

		public int Width
		{
			get
			{
				return this.pixels.GetLength(1);
			}
		}

		public int Height
		{
			get
			{
				return this.pixels.GetLength(0);
			}
		}
	}
}