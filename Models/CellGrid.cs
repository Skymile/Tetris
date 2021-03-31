using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
	// View ViewModel Model Support

	// MainWindow
	//  MainWindowView
	//  MainWindowVM
	// GameWindow
	// Model



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
			var hashX = Current.Select(i => i.X).ToHashSet();
			var hashY = Current.Select(i => i.Y).ToHashSet();

			for (int y = Config.GridHeight - 2; y >= 0; y--)
				for (int x = 0; x < Config.GridWidth; x++)
					if (
						hashX.Contains(x) &&
						hashY.Contains(y) &&
						cells[y + 1, x] == null &&
						cells[y, x] != null
						)
					{
						Move(cells[y, x], 0, 1);
					}
		}

		private void Move(Cell f, int x, int y)
		{
			cells[f.Y + y, f.X + x] = new Cell
			{
				X = f.X + x,
				Y = f.Y + y,
				Background = f.Background
			};
			cells[f.Y, f.X] = null;

			foreach (var cell in Current)
				if (cell.X == f.X && cell.Y == f.Y)
				{
					cell.Y += y;
					cell.X += x;
				}
		}

		public void Reset()
		{
			for (int y = 0; y < Config.GridHeight; y++)
				for (int x = 0; x < Config.GridWidth; x++)
					cells[y, x] = null;
		}

		public void MoveRight()
		{
			bool can = true;
			for (int y = Config.GridHeight - 1; y >= 0; y--)
				for (int x = Config.GridWidth - 1; x >= 0; x--)
					if (cells[y, x] != null)
						if (x >= Config.GridWidth - 1 || cells[y, x + 1] != null)
							can = false;

			if (can)
				for (int y = Config.GridHeight - 1; y >= 0; y--)
					for (int x = Config.GridWidth - 2; x >= 0; x--)
						if (cells[y, x] != null && cells[y, x + 1] == null)
						{
							Move(cells[y, x], 1, 0);
						}
		}

		public void MoveLeft()
		{
			bool can = true;
			for (int y = Config.GridHeight - 1; y >= 0; y--)
				for (int x = 0; x < Config.GridWidth; x++)
					if (cells[y, x] != null)
						if (x <= 0 || cells[y, x - 1] != null)
							can = false;

			if (can)
				for (int y = Config.GridHeight - 1; y >= 0; y--)
					for (int x = 1; x < Config.GridWidth; x++)
						if (cells[y, x] != null && cells[y, x - 1] == null)
						{
							Move(cells[y, x], -1, 0);
						}
		}

		public void Add(Block block)
		{
			block.Cells.ForEach(Add);
			Current = block;
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
}
