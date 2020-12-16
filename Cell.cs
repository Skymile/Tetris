using System.Windows.Media;

namespace WpfTetris
{
	public class Cell
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Brush Background { get; set; } = Config.NonEmptyCellBg;
	}
}
