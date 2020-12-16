using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Tetris
{
	// MVC  Model View Controller
	// Model, View
	// Controller - Kontroluje przebieg aplikacji

	// MVCVM
	// MVVM Model View ViewModel
	//  Model - Klasy które składają dane
	//  View  - Widoki, obsługuje wyświetlanie
	//  ViewModel - Łączy Model z View, logika

	// View:
	//   MainWindow
	//   GameWindow
	// VM:
	//   MainWindowVM
	//   GameWindowVM
	// Model
	//   Cell - pojedyncza komórka X Y Background | struktura
	//   Block - zgrupowanie komórek, obracanie
	// Inne:
	//   Config     - Odpowiedzialna za skróty klawiszowe, opcje
	//   Serializer - Wczytywanie/zapisywanie gry
	//   Grid       - Odpowiedzialna za kontrole Cells, opadanie
	//   Randomizer - Odpowiedzialna za losowanie koloru, losowanie klocka

	public class CellGrid
	{
		public Block Current { get; set; }

		public void Fall()
		{
			for (int y = Config.GridHeight - 2; y >= 0; y--)
				for (int x = 0; x < Config.GridWidth; x++)
					if (cells[y + 1, x] == null && cells[y, x] != null)
					{
						var cell = cells[y, x];
						cells[y + 1, x] = new Cell
						{
							X = cell.X,
							Y = cell.Y + 1,
							Background = cell.Background
						};
						cells[y, x] = null;
					}
		}

		public void Add(Cell cell) => cells[cell.Y, cell.X] = cell;

		public IEnumerable<Cell> GetCells()
		{
			for (int y = 0; y < Config.GridHeight; y++)
				for (int x = 0; x < Config.GridWidth; x++)
					if (cells[y, x] != null)
						yield return cells[y, x];
		}

		private readonly Cell[,] cells = new Cell[Config.GridHeight, Config.GridWidth];
	}

	public class Block
	{
		public List<Cell> Cells { get; } = new List<Cell>();

		public Block RotateLeft()
		{
			return this;
		}

		public Block RotateRight()
		{
			return this;
		}
	}

	public static class Config
	{
		public const int GridWidth = 15;
		public const int GridHeight = 25;

		public const int ScoreTick = 100;

		public static readonly Brush EmptyCellBg = Brushes.LightGray;
		public static readonly Brush NonEmptyCellBg = Brushes.Black;

		public const int CellWidth = 40;
		public const int CellHeight = 40;

		public const int GridMargin = 2;

		public const int InitialScore = 400;
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this;
			this.AddScore = new Command(() => Score += Config.ScoreTick);
			this.SubScore = new Command(() => Score -= Config.ScoreTick);

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

			grid.Add(new Cell { X = 0, Y = 0 });
			grid.Add(new Cell { X = 1, Y = 1 });
			grid.Add(new Cell { X = 4, Y = 2 });
			Refresh();
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

		public event PropertyChangedEventHandler PropertyChanged;

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			grid.Fall();
			Refresh();
		}
	}
}
