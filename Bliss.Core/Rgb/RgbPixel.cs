using System.Runtime.InteropServices;

namespace Bliss.Core.Rgb
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RgbPixel : IPixel
	{
		public byte Red { get; set; }

		public byte Green { get; set; }

		public byte Blue { get; set; }

		public byte Alpha { get; set; }
	}
}