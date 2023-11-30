#nullable enable

using Data;
using UnityEngine;

namespace Views
{
	/// <summary>
	/// The visiual representation of the random generated maze cell data.
	/// </summary>
	public class Walls : MonoBehaviour
	{
		[SerializeField]
		private GameObject? _leftWall;

		[SerializeField]
		private GameObject? _rightWall;

		[SerializeField]
		private GameObject? _frontWall;

		[SerializeField]
		private GameObject? _backWall;

		[SerializeField]
		private GameObject? _fillWall;

		public void SetWalls(MazeCell mazeCell)
		{
			if (mazeCell.IsVisited)
			{
				_fillWall!.SetActive(false);
				_leftWall!.SetActive(mazeCell.Left);
				_rightWall!.SetActive(mazeCell.Right);
				_frontWall!.SetActive(mazeCell.Front);
				_backWall!.SetActive(mazeCell.Back);
			}
		}
	}
}

#nullable disable
