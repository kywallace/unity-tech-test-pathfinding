#nullable enable

using System;

namespace Collections.Interfaces
{
	/// <summary>
	/// Interface for the heap item.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IHeapItem<T> : IComparable<T>
	{
		/// <summary>
		/// Get and set the heap item index.
		/// </summary>
		int HeapIndex { get; set; }
	}
}

#nullable disable
