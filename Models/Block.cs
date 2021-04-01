using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tetris
{
	public class Block : IEnumerable<Cell>
	{
		public List<Cell> Cells { get; } = new List<Cell>();

		public IEnumerator<Cell> GetEnumerator() => Cells.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Cells.GetEnumerator();

		public Block RotateLeft()
		{
			RotateRight();
			RotateRight();
			RotateRight();
			return this;
		}

		public Block RotateRight()
		{
			int minX = Cells.Min(i => i.X);
			int maxX = Cells.Max(i => i.X);

			int minY = Cells.Min(i => i.Y);
			int maxY = Cells.Max(i => i.Y);

			//int centerX = (maxX - minX) / 2;
			//int centerY = (maxY - minY) / 2;

			foreach (var cell in this.Cells)
			{
				int x = cell.X;

				cell.X = maxY - cell.Y + minX;
				cell.Y = x - minX + minY;
			}

			return this;
		}

	}
}
// xz
// y
//

// x (0,0) => (3,0)
// z (1,0) => (3,1)
// y (0,1) => (2,0)
//
// Y = x
// X = size - y

//  yx
//   z
//
