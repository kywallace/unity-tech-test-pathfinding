#nullable enable

using Data;

namespace Services
{
	/// <summary>
	/// Interface of the maze generator.
	/// </summary>
	public interface IMazeGenerator
	{
		MazeCell[,] GenerateMaze(int x, int y);
	}
}

#nullable disable
