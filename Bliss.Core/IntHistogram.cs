using System;
using System.Collections.Generic;

namespace Bliss.Core
{
	public class IntHistogram : IHistogram
	{
		private readonly int[] values;

		public int Max { get; set; }

		public string Title { get; set; }

		public IEnumerable<int> Values
		{
			get
			{
				return this.values;
			}
		}

		public IntHistogram(string title)
		{
			this.values = new int[256];
			Max = 0;
			Title = title;
		}

		public void Add(byte index)
		{
			this.values[index]++;
			if (values[index] > Max)
			{
				Max = values[index];
			}
		}
	}
}