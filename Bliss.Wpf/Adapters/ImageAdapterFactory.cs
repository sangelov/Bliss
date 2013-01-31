using System;
using Bliss.Core;
using Bliss.Core.Grayscale;
using Bliss.Core.Hsl;
using Bliss.Core.Rgb;

namespace Bliss.Wpf.Adapters
{
	public static class ImageAdapterFactory
	{
		public static GrayscaleImageAdapter CreateGrayscaleImageAdapter(ImageAdapter currentImage)
		{
			if (currentImage is RgbImageAdapter)
			{
				return new GrayscaleImageAdapter(new GrayscaleImage(currentImage.Image as RgbImage));
			}
			return null;
		}

		public static ImageAdapter CreateAdapter(IImage image)
		{
			if (image is GrayscaleImage)
			{
				return new GrayscaleImageAdapter(image as GrayscaleImage);
			}
			else if (image is HslImage)
			{
				return new HslImageAdapter(image as HslImage);
			}
			return null;
		}


		public static ImageAdapter Create(string path)
		{
			return new HslImageAdapter(path);
		}
	}
}