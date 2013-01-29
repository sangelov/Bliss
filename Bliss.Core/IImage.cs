using System.Collections.Generic;

namespace Bliss.Core
{
	public interface IImage : IEnumerable<IPixel>
	{
		void ApplyEqualizedHistogram(IHistogram histogram);

		IPixel[,] Pixels { get; }

		IEnumerable<IHistogram> GetHistograms();

		int Width { get; }

		int Height { get; }
	}
}