#nullable enable

using UnityEngine;

namespace Data
{
	/// <summary>
	/// This class contains data for a turn boundary in the path. This data helps smooth out the
	/// pathing of the player's object as it is following the path.
	/// </summary>
	public struct TurnBoundary
	{
		private const float VerticalLineGradient = 1e5f;

		private readonly float _gradient;
		private Vector2 _pointOnLine1;
		private Vector2 _pointOnLine2;

		private readonly float _gradientPerpendicular;

		private readonly bool _approachSide;

		public TurnBoundary(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
		{
			float dx = pointOnLine.x - pointPerpendicularToLine.x;
			float dy = pointOnLine.y - pointPerpendicularToLine.y;

			if (dx == 0)
			{
				_gradientPerpendicular = VerticalLineGradient;
			}
			else
			{
				_gradientPerpendicular = dy / dx;
			}

			if (_gradientPerpendicular == 0)
			{
				_gradient = VerticalLineGradient;
			}
			else
			{
				_gradient = -1 / _gradientPerpendicular;
			}

			_pointOnLine1 = pointOnLine;
			_pointOnLine2 = pointOnLine + new Vector2(1, _gradient);

			_approachSide = false;
			_approachSide = GetSide(pointPerpendicularToLine);
		}

		public readonly bool HasCrossedLine(Vector2 p)
		{
			return GetSide(p) != _approachSide;
		}

		private readonly bool GetSide(Vector2 p)
		{
			return (p.x - _pointOnLine1.x) * (_pointOnLine2.y - _pointOnLine1.y) > (p.y - _pointOnLine1.y) * (_pointOnLine2.x - _pointOnLine1.x);
		}
	}
}

#nullable disable
