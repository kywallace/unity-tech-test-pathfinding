#nullable enable

using Collections.Interfaces;
using UnityEngine;

namespace Data
{
	/// <summary>
	/// This class contains data for a single pathing node in the nav grid.
	/// </summary>
	public class NavGridPathNode : IHeapItem<NavGridPathNode>
	{
		public bool Walkable { get; private set; }

		public Vector3 WorldPosition { get; private set; }

		public int GridX { get; private set; }

		public int GridY { get; private set; }

		public int MovementPenalty { get; set; }

		public int GScore { get; set; }

		public int HScore { get; set; }

		public int FScore => GScore + HScore;

		public int HeapIndex { get; set; }

		public NavGridPathNode? Parent { get; set; }

		public NavGridPathNode(bool walkable, Vector3 worldPos, int gridX, int gridY, int penalty)
		{
			Walkable = walkable;
			WorldPosition = worldPos;
			GridX = gridX;
			GridY = gridY;
			MovementPenalty = penalty;
		}

		public int CompareTo(NavGridPathNode nodeToCompare)
		{
			int compare = FScore.CompareTo(nodeToCompare.FScore);
			return compare == 0 ? HScore.CompareTo(nodeToCompare.HScore) : compare;
		}
	}
}

#nullable disable
