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

		public RgbPixel(byte red, byte green, byte blue, byte alpha) : this()
		{
			this.Red = red;
			this.Green = green;
			this.Blue = blue;
			this.Alpha = alpha;
		}
  
		public RgbPixel(byte red, byte green, byte blue) : this(red, green, blue, 255)
		{
		}
	}
}