using System.Windows.Media;

namespace Tetris
{
	public class Config
	{
		public static Config Current = new Config();

		public readonly Brush EmptyCellBg = Brushes.LightGray;
		public readonly Brush NonEmptyCellBg = Brushes.Black;

		public int GridWidth { get; set; } = 15;
		public int GridHeight { get; set; } = 20;
		public int ScoreTick { get; set; } = 100;
		public int CellWidth { get; set; } = 40;
		public int CellHeight { get; set; } = 40;
		public int GridMargin { get; set; } = 2;
		public int InitialScore { get; set; } = 400;
	}
}
