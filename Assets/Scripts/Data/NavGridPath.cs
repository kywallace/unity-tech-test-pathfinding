#nullable enable

using UnityEngine;

namespace Data
{
	public class NavGridPath
	{
		public Vector3[] LookPoints { get; private set; }

		public TurnBoundary[] TurnBoundaries { get; private set; }

		public int FinishLineIndex { get; private set; }

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
