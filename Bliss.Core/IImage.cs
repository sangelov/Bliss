using System.Collections.Generic;

namespace Bliss.Core
{
	public interface IImage : IEnumerable<IPixel>
	{
		IImage ApplyEqualizedHistogram(IHistogram histogram);

		IPixel[,] Pixels { get; }

		IEnumerable<IHistogram> GetHistograms();

		int Width { get; }

		int Height { get; }
	}
}