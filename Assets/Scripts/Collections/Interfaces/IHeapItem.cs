#nullable enable

using System;

namespace Collections.Interfaces
{
	public interface IHeapItem<T> : IComparable<T>
	{
		int HeapIndex { get; set; }
	}
}

#nullable disable
