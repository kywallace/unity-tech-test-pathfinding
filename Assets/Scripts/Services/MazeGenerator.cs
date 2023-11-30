#nullable enable

using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services
{
	/// <summary>
	/// This class generates a random maze.
	/// </summary>
	public class MazeGenerator : IMazeGenerator
	{
		/// <summary>
		/// Generate a random maze from the given width and depth of the maze.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="depth"></param>
		/// <returns></returns>
		public MazeCell[,] GenerateMaze(int width, int depth)
		{
			MazeCell[,] mazeGrid = new MazeCell[width, depth];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < depth; y++)
				{
					mazeGrid[x, y] = new MazeCell(x, y);
				}
			}

			BuildMaze(mazeGrid, null, mazeGrid[0, 0]);

			return mazeGrid;
		}

		/// <summary>
		/// Builds out the random maze.
		/// </summary>
		/// <param name="mazeGrid"></param>
		/// <param name="previousCell"></param>
		/// <param name="currentCell"></param>
		private void BuildMaze(MazeCell[,] mazeGrid, MazeCell? previousCell, MazeCell currentCell)
		{
			currentCell.Visit();
			ClearWalls(previousCell, currentCell);

			MazeCell nextCell;

			do
			{
				nextCell = GetNextUnvisitedCell(mazeGrid, currentCell);

				if (nextCell != null)
				{
					BuildMaze(mazeGrid, currentCell, nextCell);
				}
			} 
			while (nextCell != null);
		}

		/// <summary>
		/// Gets a random next unvisited maze cell from the current name cell.
		/// </summary>
		/// <param name="mazeGrid"></param>
		/// <param name="currentCell"></param>
		/// <returns></returns>
		private MazeCell GetNextUnvisitedCell(MazeCell[,] mazeGrid, MazeCell currentCell)
		{
			List<MazeCell> unvisitedCells = GetUnvisitedCells(mazeGrid, currentCell);
			return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
		}

		/// <summary>
		/// Get a list of unvisited maze cells around the current maze cell.
		/// </summary>
		/// <param name="mazeGrid"></param>
		/// <param name="currentCell"></param>
		/// <returns></returns>
		private List<MazeCell> GetUnvisitedCells(MazeCell[,] mazeGrid, MazeCell currentCell)
		{
			List<MazeCell> unvisitedCells = new();

			int x = currentCell.X;
			int y = currentCell.Y;

			if (x + 1 < mazeGrid.GetLength(0))
			{
				MazeCell cellToRight = mazeGrid[x + 1, y];
				if (cellToRight.IsVisited == false)
				{
					unvisitedCells.Add(cellToRight);
				}
			}

			if (x - 1 >= 0)
			{
				MazeCell cellToLeft = mazeGrid[x - 1, y];
				if (cellToLeft.IsVisited == false)
				{
					unvisitedCells.Add(cellToLeft);
				}
			}

			if (y + 1 < mazeGrid.GetLength(1))
			{
				MazeCell cellToFront = mazeGrid[x, y + 1];
				if (cellToFront.IsVisited == false)
				{
					unvisitedCells.Add(cellToFront);
				}
			}

			if (y - 1 >= 0)
			{
				MazeCell cellToBack = mazeGrid[x, y - 1];
				if (cellToBack.IsVisited == false)
				{
					unvisitedCells.Add(cellToBack);
				}
			}

			return unvisitedCells;
		}

		/// <summary>
		/// Clear maze cell walls give the previous and current maze cell.
		/// </summary>
		/// <param name="previousCell"></param>
		/// <param name="currentCell"></param>
		private void ClearWalls(MazeCell? previousCell, MazeCell currentCell)
		{
			if (previousCell == null)
			{
				return;
			}

			if (previousCell.X < currentCell.X)
			{
				previousCell.ClearRight();
				currentCell.ClearLeft();
				return;
			}

			if (previousCell.X > currentCell.X)
			{
				previousCell.ClearLeft();
				currentCell.ClearRight();
				return;
			}

			if (previousCell.Y < currentCell.Y)
			{
				previousCell.ClearFront();
				currentCell.ClearBack();
				return;
			}

			if (previousCell.Y > currentCell.Y)
			{
				previousCell.ClearBack();
				currentCell.ClearFront();
				return;
			}
		}
	}
}

#nullable disable
