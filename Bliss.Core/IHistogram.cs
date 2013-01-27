using System.Collections.Generic;

namespace Bliss.Core
{
	public interface IHistogram
	{
		IEnumerable<int> Values { get; }

		int Max { get; }

		string Title { get; }
	}
}