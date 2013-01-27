using System;
using System.Linq;
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
			byte[] array = new byte[image.Width * image.Height];
			byte[] src = image.Pixels.Cast<GrayscalePixel>().Select(x => x.Level).ToArray();
			Buffer.BlockCopy(src, 0, array, 0, image.Width * image.Height);
			this.source = BitmapSource.Create(image.Width, image.Height, 96, 96, PixelFormats.Gray8, null, array, image.Width);
		}
	}
}