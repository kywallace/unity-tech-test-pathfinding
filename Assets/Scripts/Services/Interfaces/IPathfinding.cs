#nullable enable

using Cysharp.Threading.Tasks;
using Data;
using System.Threading;

namespace Services.Interfaces
{
	/// <summary>
	/// Interface for the pathfinding.
	/// </summary>
	public interface IPathfinding
	{
		UniTask<PathResult> FindPathAsyn(PathContext context, CancellationToken token);
	}
}

#nullable disable
