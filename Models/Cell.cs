using System.Diagnostics;
using System.Windows.Media;

namespace Tetris
{
	[DebuggerDisplay("{X} {Y}")]
	public class Cell
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Brush Background { get; set; } = Config.NonEmptyCellBg;
	}
}
