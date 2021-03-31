using System.Windows.Media;

namespace Tetris
{
	public static class Config
	{
		public const int GridWidth = 15;
		public const int GridHeight = 20;

		public const int ScoreTick = 100;

		public static readonly Brush EmptyCellBg = Brushes.LightGray;
		public static readonly Brush NonEmptyCellBg = Brushes.Black;

		public const int CellWidth = 40;
		public const int CellHeight = 40;

		public const int GridMargin = 2;

		public const int InitialScore = 400;
	}
}
