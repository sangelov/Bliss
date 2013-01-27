namespace Bliss.Core.Grayscale
{
	public struct GrayscalePixel : IPixel
	{
		public byte Level { get; set; }
		
		public GrayscalePixel(byte level) : this()
		{
			Level = level;
		}
	}
}