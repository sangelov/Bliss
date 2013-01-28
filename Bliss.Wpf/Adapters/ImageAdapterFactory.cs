using System;
using Bliss.Core;
using Bliss.Core.Grayscale;
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
			return null;
		}

		public static ImageAdapter Create(string path)
		{
			return new RgbImageAdapter(path);
		}
	}
}