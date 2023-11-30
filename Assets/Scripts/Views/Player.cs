#nullable enable

using Cysharp.Threading.Tasks;
using Data;
using Services;
using Services.Interfaces;
using System.Threading;
using UnityEngine;

namespace Views
{
	public class Player : MonoBehaviour
	{
		private const float PathUpdateMoveThreshold = 0.5f;

		[SerializeField]
		private float _speed = 15;

		[SerializeField]
		private float _turnSpeed = 15;

		[SerializeField]
		private float _turnDistance = 1;

		private NavGridPath? _path;
		private CancellationTokenSource? _cts;

		void Start()
		{
			UpdatePath(this.GetCancellationTokenOnDestroy()).Forget();
		}

		private async UniTaskVoid UpdatePath(CancellationToken token)
		{
			float sqrMoveThreshold = PathUpdateMoveThreshold * PathUpdateMoveThreshold;
			Vector3 targetPosOld = transform.position;

			while (true)
			{
				await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0), cancellationToken: token);

				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out RaycastHit hitInfo))
				{
					if ((hitInfo.point - targetPosOld).sqrMagnitude > sqrMoveThreshold)
					{
						targetPosOld = hitInfo.point;
						PathResult result = await ServiceLocator.Instance.GetService<IPathfinding>().FindPathAsyn(
							new PathContext(transform.position, hitInfo.point),
							this.GetCancellationTokenOnDestroy()
						);

						if (result.Success)
						{
							_path = new NavGridPath(result.Path, transform.position, _turnDistance);

							_cts?.Cancel();
							_cts?.Dispose();
							_cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
							FollowPathAsync(_cts.Token).Forget();
						}
					}
				}
			}
		}

		private async UniTaskVoid FollowPathAsync(CancellationToken token)
		{
			bool followingPath = true;
			int pathIndex = 0;
			float speedPercent = 1;

			Vector3 initDirection = _path!.LookPoints[0];
			initDirection.y = transform.position.y; // Only rotate on y axis.
			transform.LookAt(initDirection);

			while (followingPath)
			{
				Vector2 pos2D = new(transform.position.x, transform.position.z);
				while (_path.TurnBoundaries[pathIndex].HasCrossedLine(pos2D))
				{
					if (pathIndex == _path.FinishLineIndex)
					{
						followingPath = false;
						break;
					}
					else
					{
						pathIndex++;
					}
				}

				if (followingPath)
				{
					Vector3 direction = _path.LookPoints[pathIndex] - transform.position;
					direction.y = 0.0f; // Only rotate on y axis.
					Quaternion rotation = Quaternion.LookRotation(direction);
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _turnSpeed * Time.deltaTime);
					transform.Translate(_speed * speedPercent * Time.deltaTime * Vector3.forward, Space.Self);
				}

				await UniTask.NextFrame(token);
			}
		}
	}
}

#nullable disable
