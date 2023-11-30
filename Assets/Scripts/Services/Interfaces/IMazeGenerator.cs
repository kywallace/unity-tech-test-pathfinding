#nullable enable

using Data;

namespace Services
{
	public interface IMazeGenerator
	{
		MazeCell[,] GenerateMaze(int x, int y);
	}
}

#nullable disable
