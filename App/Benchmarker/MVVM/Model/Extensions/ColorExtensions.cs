using System.Windows.Media;

namespace Benchmarker.MVVM.Model
{
	public static class ColorExtensions
	{
		public static Color LerpColor(Color startColor, Color endColor, double t)
		{
			byte red = (byte)(startColor.R + (endColor.R - startColor.R) * t);
			byte green = (byte)(startColor.G + (endColor.G - startColor.G) * t);
			byte blue = (byte)(startColor.B + (endColor.B - startColor.B) * t);
			return Color.FromRgb(red, green, blue);
		}
	}
}
