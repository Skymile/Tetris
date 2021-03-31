using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Tetris
{
	public class MainWindowVM : INotifyPropertyChanged
	{
		public MainWindowVM(Grid mainGrid)
		{
			this.mainGrid = mainGrid;
			Reset();
		}

		public void Cycle()
		{
			grid.Fall();
			Refresh();
		}

		public void Refresh()
		{
			foreach (var i in borders)
				i.Background = Brushes.LightGray;
			foreach (var cell in grid.GetCells())
				borders[cell.X, cell.Y].Background = cell.Background;
		}

		public void Reset()
		{
			this.Score = 400;
			this.AddScore = new Command(() => Score += Config.ScoreTick);
			this.SubScore = new Command(() => Score -= Config.ScoreTick);

			this.mainGrid.ColumnDefinitions.Clear();
			this.mainGrid.RowDefinitions.Clear();

			for (int i = 0; i < Config.GridWidth; i++)
				this.mainGrid.ColumnDefinitions.Add(
					new ColumnDefinition { Width = new GridLength(Config.CellWidth + Config.GridMargin, GridUnitType.Pixel) }
				);
			for (int i = 0; i < Config.GridHeight; i++)
				this.mainGrid.RowDefinitions.Add(
					new RowDefinition { Height = new GridLength(Config.CellHeight + Config.GridMargin, GridUnitType.Pixel) }
				);

			for (int x = 0; x < Config.GridWidth; x++)
				for (int y = 0; y < Config.GridHeight; y++)
				{
					var border = new Border()
					{
						Background = Config.EmptyCellBg,
						Width = Config.CellWidth,
						Height = Config.CellHeight
					};

					this.mainGrid.Children.Add(border);
					Grid.SetRow(border, y);
					Grid.SetColumn(border, x);
					borders[x, y] = border;
				}

			grid.Reset();

			var block = new Block();
			block.Cells.Add(new Cell { X = 0, Y = 0 });
			block.Cells.Add(new Cell { X = 1, Y = 1 });
			block.Cells.Add(new Cell { X = 4, Y = 2 });

			grid.Add(block);
			Refresh();
		}

		public void MoveRight() => grid.MoveRight();
		public void MoveLeft() => grid.MoveLeft();

		public int Score
		{
			get => this.score;
			set
			{
				this.score = value;
				PropertyChanged?.Invoke(
					this,
					new PropertyChangedEventArgs(nameof(this.Score))
				);
			}
		}

		public ICommand AddScore { get; set; }
		public ICommand SubScore { get; set; }

		public ICommand Load { get; set; }
		public ICommand Save { get; set; }
		public ICommand Pause { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;


		private Grid mainGrid { get; }
		private CellGrid grid = new CellGrid();
		private readonly Border[,] borders = new Border[Config.GridWidth, Config.GridHeight];
		private int score = Config.InitialScore;
	}
}