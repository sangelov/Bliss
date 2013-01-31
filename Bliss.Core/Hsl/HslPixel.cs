using System;
using System.Runtime.InteropServices;

namespace Bliss.Core.Hsl
{
	[StructLayout(LayoutKind.Sequential)]
	public struct HslPixel : IPixel
	{
		private readonly float hue;
		private readonly float saturation;
		private readonly float luminance;
		private byte? normalizedValue;

		public HslPixel(float hue, float saturation, float luminance)
		{
			this.hue = hue;
			this.saturation = saturation;
			this.luminance = luminance;
			this.normalizedValue = null;
		}

		public float Hue
		{
			get
			{
				return this.hue;
			}
		}

		public float Saturation
		{
			get
			{
				return this.saturation;
			}
		}

		public float Luminance
		{
			get
			{
				return this.luminance;
			}
		}


		/// <summary>
		/// returns the luminance [0..1] normalized to 100
		/// </summary>
		public byte NormalizedLuminance
		{
			get
			{
				if (!this.normalizedValue.HasValue)
				{
					this.normalizedValue = Convert.ToByte(Math.Round(this.luminance * 100, MidpointRounding.AwayFromZero));
				}
				return this.normalizedValue.Value;
			}
		}
	}
}