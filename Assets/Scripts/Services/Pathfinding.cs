#nullable enable

using Collections;
using Cysharp.Threading.Tasks;
using Data;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Views;

namespace Services
{
	public class Pathfinding : IPathfinding
	{
		private readonly NavGrid _grid;

		public Pathfinding(NavGrid grid)
		{
			_grid = grid;
		}

		public async UniTask<PathResult> FindPathAsyn(PathContext context, CancellationToken token)
		{
			Vector3[] waypoints = new Vector3[0];
			bool pathSuccess = false;

			NavGridPathNode startNode = _grid.NodeFromWorldPoint(context.PathStart);
			NavGridPathNode targetNode = _grid.NodeFromWorldPoint(context.PathEnd);
			startNode.Parent = startNode;

			if (startNode.Walkable && targetNode.Walkable)
			{
				await UniTask.RunOnThreadPool(() =>
				{
					Heap<NavGridPathNode> openSet = new(_grid.MaxSize);
					HashSet<NavGridPathNode> closedSet = new();
					openSet.Add(startNode);

					while (openSet.Count > 0)
					{
						NavGridPathNode currentNode = openSet.RemoveFirst();
						closedSet.Add(currentNode);

						if (currentNode == targetNode)
						{
							pathSuccess = true;
							break;
						}

						foreach (NavGridPathNode neighbour in _grid.GetNeighbours(currentNode))
						{
							if (!neighbour.Walkable || closedSet.Contains(neighbour))
							{
								continue;
							}

							int newMovementCostToNeighbour = currentNode.GScore + GetDistance(currentNode, neighbour) + neighbour.MovementPenalty;
							if (newMovementCostToNeighbour < neighbour.GScore || !openSet.Contains(neighbour))
							{
								neighbour.GScore = newMovementCostToNeighbour;
								neighbour.HScore = GetDistance(neighbour, targetNode);
								neighbour.Parent = currentNode;

								if (!openSet.Contains(neighbour))
								{
									openSet.Add(neighbour);
								}
								else
								{
									openSet.UpdateItem(neighbour);
								}
							}
						}
					}
				}, cancellationToken: token);
			}

			if (pathSuccess)
			{
				waypoints = RetracePath(startNode, targetNode);
				pathSuccess = waypoints.Length > 0;
			}

			return new PathResult(pathSuccess, waypoints);
		}

		private Vector3[] RetracePath(NavGridPathNode startNode, NavGridPathNode endNode)
		{
			List<NavGridPathNode> path = new();
			NavGridPathNode? currentNode = endNode;

			while (currentNode != null && currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.Parent;
			}

			Vector3[] waypoints = SimplifyPath(path);
			Array.Reverse(waypoints);
			return waypoints;
		}

		private Vector3[] SimplifyPath(List<NavGridPathNode> path)
		{
			List<Vector3> waypoints = new();
			Vector2 directionOld = Vector2.zero;

			for (int i = 1; i < path.Count; i++)
			{
				Vector2 directionNew = new(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
				if (directionNew != directionOld)
				{
					waypoints.Add(path[i].WorldPosition);
				}
				directionOld = directionNew;
			}
			return waypoints.ToArray();
		}

		private int GetDistance(NavGridPathNode nodeA, NavGridPathNode nodeB)
		{
			int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
			int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

			return dstX > dstY ? 14 * dstY + 10 * (dstX - dstY) : 14 * dstX + 10 * (dstY - dstX);
		}
	}
}

#nullable disable
