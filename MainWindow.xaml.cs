using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Tetris
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this;
			Reset();
		}

		private CellGrid grid = new CellGrid();

		public void Refresh()
		{
			foreach (var i in borders)
				i.Background = Brushes.LightGray;
			foreach (var cell in grid.GetCells())
				borders[cell.X, cell.Y].Background = cell.Background;
		}

		private readonly Border[,] borders = new Border[Config.GridWidth, Config.GridHeight];

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
		private int score = Config.InitialScore;

		public ICommand AddScore { get; set; }
		public ICommand SubScore { get; set; }

		public ICommand Load  { get; set; }
		public ICommand Save  { get; set; }
		public ICommand Pause { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private void Cycle()
		{
			grid.Fall();
			Refresh();
		}

		private void Reset()
		{
			this.Score = 400;
			this.AddScore = new Command(() => Score += Config.ScoreTick);
			this.SubScore = new Command(() => Score -= Config.ScoreTick);

			this.MainGrid.ColumnDefinitions.Clear();
			this.MainGrid.RowDefinitions.Clear();

			for (int i = 0; i < Config.GridWidth; i++)
				this.MainGrid.ColumnDefinitions.Add(
					new ColumnDefinition { Width = new GridLength(Config.CellWidth + Config.GridMargin, GridUnitType.Pixel) }
				);
			for (int i = 0; i < Config.GridHeight; i++)
				this.MainGrid.RowDefinitions.Add(
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

					this.MainGrid.Children.Add(border);
					Grid.SetRow(border, y);
					Grid.SetColumn(border, x);
					borders[x, y] = border;
				}

			grid.Reset();
			grid.Add(new Cell { X = 0, Y = 0 });
			grid.Add(new Cell { X = 1, Y = 1 });
			grid.Add(new Cell { X = 4, Y = 2 });
			Refresh();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.R:
					Reset();
					break;
				case Key.P:
					Pause.Execute(null);
					break;
				case Key.S:
					Cycle();
					break;
				case Key.D:
					grid.MoveRight();
					Refresh();
					break;
				case Key.A:
					grid.MoveLeft();
					Refresh();
					break;
				default:
					break;
			}
		}
	}
}
