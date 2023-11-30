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
	/// <summary>
	/// This is an implementation of the A* pathing algorithm.
	/// </summary>
	public class Pathfinding : IPathfinding
	{
		private readonly NavGrid _grid;

		/// <summary>
		/// Constructor to assign the nav grid that the pathing algorithm will find paths on.
		/// </summary>
		/// <param name="grid"></param>
		public Pathfinding(NavGrid grid)
		{
			_grid = grid;
		}
		
		/// <summary>
		/// Find the path of the give path context.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async UniTask<PathResult> FindPathAsyn(PathContext context, CancellationToken token)
		{
			Vector3[] waypoints = new Vector3[0];
			bool pathSuccess = false;

			NavGridPathNode startNode = _grid.NodeFromWorldPoint(context.PathStart);
			NavGridPathNode targetNode = _grid.NodeFromWorldPoint(context.PathEnd);
			startNode.Parent = startNode;

			if (startNode.Walkable && targetNode.Walkable)
			{
				// Run this on a separate thread to not affect the FPS performance of the game. If this is a single threaded device
				// then this will time slice the work to again not affect the FPS performance of the game.
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

						foreach (NavGridPathNode neighbor in _grid.GetNeighbor(currentNode))
						{
							if (!neighbor.Walkable || closedSet.Contains(neighbor))
							{
								continue;
							}

							int newMovementCostToNeighbor = currentNode.GScore + GetDistance(currentNode, neighbor) + neighbor.MovementPenalty;
							if (newMovementCostToNeighbor < neighbor.GScore || !openSet.Contains(neighbor))
							{
								neighbor.GScore = newMovementCostToNeighbor;
								neighbor.HScore = GetDistance(neighbor, targetNode);
								neighbor.Parent = currentNode;

								if (!openSet.Contains(neighbor))
								{
									openSet.Add(neighbor);
								}
								else
								{
									openSet.UpdateItem(neighbor);
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

		/// <summary>
		/// Retraces the found path and simplify the found path to produce the waypoints that the
		/// player's object will follow.
		/// </summary>
		/// <param name="startNode"></param>
		/// <param name="endNode"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Simplify the path by removing path nodes that don't change the direction of
		/// the path.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
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
