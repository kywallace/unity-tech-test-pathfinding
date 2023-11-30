#nullable enable

using UnityEngine;

namespace Data
{
	/// <summary>
	/// This class contains data for the path context that the pathfinding will find a
	/// path for.
	/// </summary>
	public struct PathContext
	{
		public Vector3 PathStart;
		public Vector3 PathEnd;

		public PathContext(Vector3 pathStart, Vector3 pathEnd)
		{
			PathStart = pathStart;
			PathEnd = pathEnd;
		}
	}
}

#nullable disable
