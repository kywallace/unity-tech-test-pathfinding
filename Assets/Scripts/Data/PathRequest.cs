#nullable enable

using UnityEngine;

namespace Data
{
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
