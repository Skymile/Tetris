using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tetris
{
	public class Block : IEnumerable<Cell>
	{
		public List<Cell> Cells { get; } = new List<Cell>();

		public IEnumerator<Cell> GetEnumerator() => Cells.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Cells.GetEnumerator();

		public Block RotateLeft()
		{
			return this;
		}

		public Block RotateRight()
		{
			return this;
		}

	}
}
