using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bliss.Core.Grayscale;

namespace Bliss.Wpf.Adapters
{
	public class GrayscaleImageAdapter : ImageAdapter
	{
		public GrayscaleImageAdapter(GrayscaleImage image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			this.image = image;
			byte[] src = image.Pixels.Cast<GrayscalePixel>().Select(x => x.Level).ToArray();
			this.source = BitmapSource.Create(image.Width, image.Height, 96, 96, PixelFormats.Gray8, null, src, image.Width);
		}
	}
}