using System;
using System.Collections.Generic;

namespace Bliss.Core
{
	public class IntHistogram : IHistogram
	{
		private readonly IImage image;

		private int[] values;
		private int[] equalizedValues;

		public int Max { get; set; }

		public string Title { get; set; }

		public IImage Image
		{
			get
			{
				return this.image;
			}
		}

		public IEnumerable<int> Values
		{
			get
			{
				return this.values;
			}
		}

		public IEnumerable<int> EqualizedValues
		{
			get
			{
				if (this.equalizedValues == null)
				{
					Equalize();
				}
				return this.equalizedValues;
			}
		}

		public IntHistogram(IImage image, string title)
		{
			this.image = image;
			Max = 0;
			Title = title;
		}

		public void Add(byte index)
		{
			if (this.values == null)
			{
				this.values = new int[256];
			}

			this.values[index]++;
			if (values[index] > Max)
			{
				Max = values[index];
			}
		}

		private void Equalize()
		{
			if (this.values == null)
			{
				throw new Exception("histogram hasn't been initialized");
			}

			// cumulative distribution function 
			int[] cdf = new int[this.values.Length];

			int sum = 0;
			int min = int.MaxValue;
			for (int i = 0; i < values.Length; i++)
			{
				sum += this.values[i];
				if (sum < min)
				{
					min = sum;
				}
				cdf[i] = sum;
			}

			this.equalizedValues = new int[this.values.Length];

			for (int i = 0; i < this.values.Length; i++)
			{
				double up = cdf[i] - min;
				double down = (this.image.Width * this.image.Height) - min;
				double equalized = (up / down) * 255;
				this.equalizedValues[i] = (int)Math.Round(equalized, MidpointRounding.AwayFromZero);
			}
		}
	}
}