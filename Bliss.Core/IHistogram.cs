using System.Collections.Generic;

namespace Bliss.Core
{
	public interface IHistogram
	{
		IImage Image { get; }

		IEnumerable<int> Values { get; }

		IEnumerable<int> EqualizedValues { get; }

		int Max { get; }

		string Title { get; }
	}
}