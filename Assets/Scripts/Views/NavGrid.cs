#nullable enable

using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Views
{
	public class NavGrid : MonoBehaviour
	{
		private const int BlurSize = 3;

		[SerializeField]
		private LayerMask _unpathableMask;

		[SerializeField]
		private float _nodeRadius;

		[SerializeField]
		private int _obstacleProximityPenalty = 10;

		private NavGridPathNode[,]? _grid;

		private Vector3 _gridWorldSize;
		private float _nodeDiameter;
		private int _gridSizeX;
		private int _gridSizeY;

		private int _penaltyMin = int.MaxValue;
		private int _penaltyMax = int.MinValue;

		public int MaxSize => _gridSizeX * _gridSizeY;

		void Awake()
		{
			MeshRenderer? renderer = GetComponent<MeshRenderer>();
			_gridWorldSize = renderer!.bounds.size;

			_nodeDiameter = _nodeRadius * 2;
			_gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
			_gridSizeY = Mathf.RoundToInt(_gridWorldSize.z / _nodeDiameter);
		}

		public void CreateGrid()
		{
			_grid = new NavGridPathNode[_gridSizeX, _gridSizeY];
			Vector3 worldBottomLeft = transform.position - Vector3.right * _gridWorldSize.x / 2 - Vector3.forward * _gridWorldSize.z / 2;

			for (int x = 0; x < _gridSizeX; x++)
			{
				for (int y = 0; y < _gridSizeY; y++)
				{
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.forward * (y * _nodeDiameter + _nodeRadius);
					bool walkable = !(Physics.CheckSphere(worldPoint, _nodeRadius, _unpathableMask));

					int movementPenalty = 0;
					if (!walkable)
					{
						movementPenalty += _obstacleProximityPenalty;
					}

					_grid[x, y] = new NavGridPathNode(walkable, worldPoint, x, y, movementPenalty);
				}
			}

			BlurPenaltyMap(BlurSize);
		}

		void BlurPenaltyMap(int blurSize)
		{
			int kernelSize = blurSize * 2 + 1;
			int kernelExtents = (kernelSize - 1) / 2;

			int[,] penaltiesHorizontalPass = new int[_gridSizeX, _gridSizeY];
			int[,] penaltiesVerticalPass = new int[_gridSizeX, _gridSizeY];

			for (int y = 0; y < _gridSizeY; y++)
			{
				for (int x = -kernelExtents; x <= kernelExtents; x++)
				{
					int sampleX = Mathf.Clamp(x, 0, kernelExtents);
					penaltiesHorizontalPass[0, y] += _grid![sampleX, y].MovementPenalty;
				}

				for (int x = 1; x < _gridSizeX; x++)
				{
					int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, _gridSizeX);
					int addIndex = Mathf.Clamp(x + kernelExtents, 0, _gridSizeX - 1);

					penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - _grid![removeIndex, y].MovementPenalty + _grid[addIndex, y].MovementPenalty;
				}
			}

			for (int x = 0; x < _gridSizeX; x++)
			{
				for (int y = -kernelExtents; y <= kernelExtents; y++)
				{
					int sampleY = Mathf.Clamp(y, 0, kernelExtents);
					penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
				}

				int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
				_grid![x, 0].MovementPenalty = blurredPenalty;

				for (int y = 1; y < _gridSizeY; y++)
				{
					int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, _gridSizeY);
					int addIndex = Mathf.Clamp(y + kernelExtents, 0, _gridSizeY - 1);

					penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
					blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
					_grid[x, y].MovementPenalty = blurredPenalty;

					if (blurredPenalty > _penaltyMax)
					{
						_penaltyMax = blurredPenalty;
					}
					if (blurredPenalty < _penaltyMin)
					{
						_penaltyMin = blurredPenalty;
					}
				}
			}
		}

		public List<NavGridPathNode> GetNeighbours(NavGridPathNode node)
		{
			List<NavGridPathNode> neighbours = new();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (x == 0 && y == 0)
					{
						continue;
					}

					int checkX = node.GridX + x;
					int checkY = node.GridY + y;

					if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
					{
						neighbours.Add(_grid![checkX, checkY]);
					}
				}
			}

			return neighbours;
		}

		public NavGridPathNode NodeFromWorldPoint(Vector3 worldPosition)
		{
			float percentX = Mathf.Clamp01((worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x);
			float percentY = Mathf.Clamp01((worldPosition.z + _gridWorldSize.z / 2) / _gridWorldSize.z);
			int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
			int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
			return _grid![x, y];
		}
	}
}

#nullable disable
