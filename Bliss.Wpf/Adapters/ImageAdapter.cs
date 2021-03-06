using System;
using System.Linq;
using System.Windows.Media.Imaging;
using Bliss.Core;

namespace Bliss.Wpf.Adapters
{
	public class ImageAdapter
	{
		protected IImage image;
		protected BitmapSource source;

		public BitmapSource Source
		{
			get
			{
				return this.source;
			}
		}

		public IImage Image
		{
			get
			{
				return this.image;
			}
		}
	}
}