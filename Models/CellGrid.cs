using System;
using System.Collections;
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

	public class CellGrid : IEnumerable<Cell>
	{
		public Block Current { get; set; }

		public void Fall()
		{
			if (Current is null)
				return;

			var hashX = Current.Select(i => i.X).ToHashSet();
			var hashY = Current.Select(i => i.Y).ToHashSet();

			if (hashY.Contains(Config.Current.GridHeight - 1))
			{
				foreach (var i in Current)
					i.IsBlock = false;
				Current = null;
			}

			for (int y = Config.Current.GridHeight - 2; y >= 0; y--)
				for (int x = 0; x < Config.Current.GridWidth; x++)
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
			if (Current is null)
				return;

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
			Current = null;
			for (int y = 0; y < Config.Current.GridHeight; y++)
				for (int x = 0; x < Config.Current.GridWidth; x++)
					cells[y, x] = null;
		}

		public void MoveRight()
		{
			HashSet<int> setX = Current.Select(i => i.X).ToHashSet();
			HashSet<int> setY = Current.Select(i => i.Y).ToHashSet();

			bool can = !setX.Contains(Config.Current.GridWidth - 1);
			for (int y = Config.Current.GridHeight - 1; y >= 0; y--)
				for (int x = Config.Current.GridWidth - 1; x >= 0; x--)
					if (cells[y, x] != null)
						if (x >= Config.Current.GridWidth - 1 || cells[y, x + 1] != null)
						{
							if (!setX.Contains(x) && !setY.Contains(y))
								can = false;
						}

			if (can)
				for (int y = Config.Current.GridHeight - 1; y >= 0; y--)
					for (int x = Config.Current.GridWidth - 2; x >= 0; x--)
						if (cells[y, x] != null && cells[y, x + 1] == null)
						{
							Move(cells[y, x], 1, 0);
						}
		}

		public void MoveLeft()
		{
			HashSet<int> setX = Current.Select(i => i.X).ToHashSet();
			HashSet<int> setY = Current.Select(i => i.Y).ToHashSet();

			bool can = !setX.Contains(0);
			for (int y = Config.Current.GridHeight - 1; y >= 0; y--)
				for (int x = 0; x < Config.Current.GridWidth; x++)
					if (cells[y, x] != null)
						if (x <= 0 || cells[y, x - 1] != null)
						{
							if (!setX.Contains(x) && !setY.Contains(y))
								can = false;
						}

			if (can)
				for (int y = Config.Current.GridHeight - 1; y >= 0; y--)
					for (int x = 1; x < Config.Current.GridWidth; x++)
						if (cells[y, x] != null && cells[y, x - 1] == null)
						{
							Move(cells[y, x], -1, 0);
						}
		}

		public void RotateRight() => Rotate(() => Current.RotateRight());
		public void RotateLeft () => Rotate(() => Current.RotateLeft ());

		private void Rotate(Func<Block> rotated)
		{
			var old = Current.ToList();
			foreach (Cell i in Current)
				cells[i.Y, i.X] = null;
			rotated();
			// Sprawdzanie czy jest miejsce
			// Current = old;
			foreach (Cell i in Current)
				cells[i.Y, i.X] = i;
		}

		public void Add(Block block)
		{
			foreach (var i in block)
				i.IsBlock = true;
			block.Cells.ForEach(Add);
			Current = block;

			cells[9, 10] = new Cell() { X = 9, Y = 10 };
		}

		public void Add(Cell cell) => cells[cell.Y, cell.X] = cell;

		public IEnumerable<Cell> GetCells()
		{
			for (int y = 0; y < Config.Current.GridHeight; y++)
				for (int x = 0; x < Config.Current.GridWidth; x++)
					if (cells[y, x] != null)
						yield return cells[y, x];
		}

		public IEnumerator<Cell> GetEnumerator() => (IEnumerator<Cell>)cells.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator()  => cells.GetEnumerator();

		private readonly Cell[,] cells = new Cell[Config.Current.GridHeight, Config.Current.GridWidth];
	}
}
