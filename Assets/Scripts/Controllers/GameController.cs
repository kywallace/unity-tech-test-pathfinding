#nullable enable

using Services;
using Services.Interfaces;
using UnityEngine;
using Views;

namespace Controllers
{
	/// <summary>
	/// The game controller sets up the game for the player. If there was a win or lose
	/// condition to the game it would be checked here and when met the player would
	/// see a win or lose screen with a would you like to play again option.
	/// </summary>
	public class GameController : MonoBehaviour
	{
		/// <summary>
		/// Reference to the maze object.
		/// </summary>
		[SerializeField]
		private Maze? _maze;

		/// <summary>
		/// Reference to the nav grid object.
		/// </summary>
		[SerializeField]
		private NavGrid? _grid;

		/// <summary>
		/// In Awake setup the service locator.
		/// </summary>
		private void Awake()
		{
			ServiceLocator.Instance.AddService<IMazeGenerator>(new MazeGenerator());
			ServiceLocator.Instance.AddService<IPathfinding>(new Pathfinding(_grid!));
		}

		/// <summary>
		/// In start setup the game for the player.
		/// </summary>
		private void Start()
		{
			_maze!.CreateMaze();
			_grid!.CreateGrid();
		}
	}
}

#nullable disable
