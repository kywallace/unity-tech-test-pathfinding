#nullable enable

namespace Data
{
	public class MazeCell
	{
		public int X { get; private set; }

		public int Y { get; private set; }

		public bool IsVisited { get; private set; } = false;

		public bool Left { get; private set; } = true;

		public bool Right { get; private set; } = true;

		public bool Front { get; private set; } = true;

		public bool Back { get; private set; } = true;

		public MazeCell(int x, int y)
		{
			X = x;
			Y = y;
		}

		public void Visit()
		{
			IsVisited = true;
		}

		public void ClearLeft()
		{
			Left = false;
		}

		public void ClearRight()
		{
			Right = false;
		}

		public void ClearFront()
		{
			Front = false;
		}

		public void ClearBack()
		{
			Back = false;
		}
	}
}

#nullable disable
