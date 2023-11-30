#nullable enable

using Collections.Interfaces;
using UnityEngine;

namespace Data
{
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
			if (compare == 0)
			{
				compare = HScore.CompareTo(nodeToCompare.HScore);
			}
			return -compare;
		}
	}
}

#nullable disable
