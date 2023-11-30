#nullable enable

using Data;
using Services;
using UnityEngine;

namespace Views
{
	/// <summary>
	/// The visiual representation of the random generated maze data.
	/// </summary>
	public class Maze : MonoBehaviour
	{
		[SerializeField]
		private Walls? _wallsPrefab;

		[SerializeField]
		private int _width;

		[SerializeField]
		private int _depth;

		[SerializeField]
		private int _scale;

		public void CreateMaze()
		{
			MazeCell[,] mazeGrid = ServiceLocator.Instance.GetService<IMazeGenerator>().GenerateMaze(_width, _depth);

			for (int x = 0; x < mazeGrid.GetLength(0); x++)
			{
				for (int y = 0; y < mazeGrid.GetLength(1); y++)
				{
					Walls? walls = Instantiate(
						_wallsPrefab,
						new Vector3(x * _scale + this.transform.position.x, 0, y * _scale + this.transform.position.z),
						Quaternion.identity,
						this.transform
					);
					walls!.SetWalls(mazeGrid[x, y]);
				}
			}
		}
	}
}

#nullable disable
