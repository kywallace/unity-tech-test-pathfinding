#nullable enable

using UnityEngine;

namespace Data
{
	/// <summary>
	/// This class contains data for the found path result from the pathfinding.
	/// </summary>
	public struct PathResult
	{
		public bool Success;
		public Vector3[] Path;

		public PathResult(bool success, Vector3[] path)
		{ 
			Success = success;
			Path = path;
		}
	}
}

#nullable disable
