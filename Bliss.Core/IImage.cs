using System.Collections.Generic;

namespace Bliss.Core
{
	public interface IImage : IEnumerable<IPixel>
	{
		IPixel[,] Pixels { get; }

		IEnumerable<IHistogram> GetHistograms();
	}
}