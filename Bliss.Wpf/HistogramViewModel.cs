using System.Windows;
using System.Windows.Media;
using Bliss.Core;

namespace Bliss.Wpf
{
	public class HistogramViewModel
	{
		private readonly IHistogram histogram;
		private readonly PointCollection points;

		public HistogramViewModel(IHistogram histogram)
		{
			this.histogram = histogram;
			this.points = new PointCollection();
			
			this.points.Add(new Point(0, histogram.Max));
			int i = 0;
			foreach(int value in histogram.Values)
			{
				points.Add(new Point(i++, histogram.Max -value));
			}
			// last point (lower-right corner)
			points.Add(new Point(i, histogram.Max));
		}

		public string Title
		{
			get
			{
				return this.histogram.Title;
			}
		}

		public PointCollection Points
		{
			get
			{
				return this.points;
			}
		}

		public Brush Color
		{
			get
			{
				string title = Title.ToUpper();
				if (title == "RED")
				{
					return new SolidColorBrush(Colors.Red);
				}
				else if (title == "GREEN")
				{
					return new SolidColorBrush(Colors.Green);
				}
				else if (title == "BLUE")
				{
					return new SolidColorBrush(Colors.Blue);
				}
				else
				{
					return new SolidColorBrush(Colors.Black);
				}
			}
		}
	}
}