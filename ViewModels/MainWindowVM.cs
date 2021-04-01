using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Tetris.IO;

namespace Tetris
{
	public class MainWindowVM : INotifyPropertyChanged
	{
		public MainWindowVM(Grid mainGrid)
		{
			this.mainGrid = mainGrid;
			this.Load = new Command(() => {
				grid.Reset();

				var block = new Block();
				block.Cells.Add(new Cell { X = 0, Y = 0 });
				block.Cells.Add(new Cell { X = 1, Y = 0 });
				block.Cells.Add(new Cell { X = 2, Y = 0 });
				block.Cells.Add(new Cell { X = 2, Y = 1 });

				//var block = Serializer.Deserialize<Block>(File.ReadAllText("save.sav"));

				grid.Add(block);

				File.WriteAllText("save.ini", string.Join('\n',
						Serializer.Serialize(grid),
						Serializer.Serialize(Config.Current)
					)
				);
				Refresh();
			});
			this.Save = new Command(() => {

			});
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
			this.AddScore = new Command(() => Score += Config.Current.ScoreTick);
			this.SubScore = new Command(() => Score -= Config.Current.ScoreTick);

			this.mainGrid.ColumnDefinitions.Clear();
			this.mainGrid.RowDefinitions.Clear();

			for (int i = 0; i < Config.Current.GridWidth; i++)
				this.mainGrid.ColumnDefinitions.Add(
					new ColumnDefinition { Width = new GridLength(Config.Current.CellWidth + Config.Current.GridMargin, GridUnitType.Pixel) }
				);
			for (int i = 0; i < Config.Current.GridHeight; i++)
				this.mainGrid.RowDefinitions.Add(
					new RowDefinition { Height = new GridLength(Config.Current.CellHeight + Config.Current.GridMargin, GridUnitType.Pixel) }
				);

			for (int x = 0; x < Config.Current.GridWidth; x++)
				for (int y = 0; y < Config.Current.GridHeight; y++)
				{
					var border = new Border()
					{
						Background = Config.Current.EmptyCellBg,
						Width = Config.Current.CellWidth,
						Height = Config.Current.CellHeight
					};

					this.mainGrid.Children.Add(border);
					Grid.SetRow(border, y);
					Grid.SetColumn(border, x);
					borders[x, y] = border;
				}

			grid.Reset();

			var block = new Block();
			block.Cells.Add(new Cell { X = 0, Y = 0 });
			block.Cells.Add(new Cell { X = 1, Y = 0 });
			block.Cells.Add(new Cell { X = 2, Y = 0 });
			block.Cells.Add(new Cell { X = 2, Y = 1 });

			grid.Add(block);
			Refresh();
		}

		public void MoveRight() => grid.MoveRight();
		public void MoveLeft() => grid.MoveLeft();
		public void RotateRight() => grid.RotateRight();
		public void RotateLeft()  => grid.RotateLeft();

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
		private readonly Border[,] borders = new Border[Config.Current.GridWidth, Config.Current.GridHeight];
		private int score = Config.Current.InitialScore;
	}
}