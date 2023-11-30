#nullable enable

using Services;
using Services.Interfaces;
using UnityEngine;
using Views;

namespace Controllers
{
	public class GameController : MonoBehaviour
	{
		[SerializeField]
		private Maze? _maze;

		[SerializeField]
		private NavGrid? _grid;

		private void Awake()
		{
			ServiceLocator.Instance.AddService<IMazeGenerator>(new MazeGenerator());
			ServiceLocator.Instance.AddService<IPathfinding>(new Pathfinding(_grid!));
		}

		private void Start()
		{
			_maze!.CreateMaze();
			_grid!.CreateGrid();
		}
	}
}

#nullable disable
