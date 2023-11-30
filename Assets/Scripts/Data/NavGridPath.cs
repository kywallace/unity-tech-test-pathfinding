#nullable enable

using UnityEngine;

namespace Data
{
	/// <summary>
	/// This class contains data for the path that the pathfinding found and the path that the
	/// player's game object will follow.
	/// </summary>
	public class NavGridPath
	{
		/// <summary>
		/// The look points that the player object will look at and head towards.
		/// </summary>
		public Vector3[] LookPoints { get; private set; }

		/// <summary>
		/// Boundary points when passed the player object will start turing towards the next
		/// look point.
		/// </summary>
		public TurnBoundary[] TurnBoundaries { get; private set; }

		/// <summary>
		/// Index of the finish line of the path.
		/// </summary>
		public int FinishLineIndex { get; private set; }

		/// <summary>
		/// Setup the look points, turn boundaries, and finish line index from the pathfinding
		/// data that the player's object will follow.
		/// </summary>
		/// <param name="waypoints"></param>
		/// <param name="startPos"></param>
		/// <param name="turnDst"></param>
		public NavGridPath(Vector3[] waypoints, Vector3 startPos, float turnDst)
		{
			LookPoints = waypoints;
			TurnBoundaries = new TurnBoundary[LookPoints.Length];
			FinishLineIndex = TurnBoundaries.Length - 1;

			Vector2 previousPoint = V3ToV2(startPos);
			for (int i = 0; i < LookPoints.Length; i++)
			{
				Vector2 currentPoint = V3ToV2(LookPoints[i]);
				Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
				Vector2 turnBoundaryPoint = (i == FinishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDst;
				TurnBoundaries[i] = new TurnBoundary(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);
				previousPoint = turnBoundaryPoint;
			}
		}

		private Vector2 V3ToV2(Vector3 v3)
		{
			return new Vector2(v3.x, v3.z);
		}
	}
}

#nullable disable
