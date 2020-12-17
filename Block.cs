using System.Collections.Generic;

namespace Tetris
{
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
}
