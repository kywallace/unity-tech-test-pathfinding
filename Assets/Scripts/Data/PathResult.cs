#nullable enable

using UnityEngine;

namespace Data
{
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
