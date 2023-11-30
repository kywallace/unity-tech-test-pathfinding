#nullable enable

using Collections.Interfaces;

namespace Collections
{
	/// <summary>
	/// Implementation of a Min Binary Heap.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Heap<T> where T : IHeapItem<T>
	{
		/// <summary>
		/// Array to store items in heap.
		/// </summary>
		private readonly T[] _items;

		/// <summary>
		/// Current count of items in the heap.
		/// </summary>
		private int _currentItemCount;

		/// <summary>
		/// Returns the current count of items in the heap.
		/// </summary>
		public int Count => _currentItemCount;

		/// <summary>
		/// Initialize and set max size of the heap.
		/// </summary>
		/// <param name="maxHeapSize"></param>
		public Heap(int maxHeapSize)
		{
			_items = new T[maxHeapSize];
		}

		/// <summary>
		/// Add item to heap and re-sort the heap.
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			item.HeapIndex = _currentItemCount;
			_items[_currentItemCount] = item;
			SortUp(item);
			_currentItemCount++;
		}

		/// <summary>
		/// Remove the min value item which is the root itme and re-sort. 
		/// </summary>
		/// <returns></returns>
		public T RemoveFirst()
		{
			T firstItem = _items[0];
			_currentItemCount--;
			_items[0] = _items[_currentItemCount];
			_items[0].HeapIndex = 0;
			SortDown(_items[0]);
			return firstItem;
		}

		/// <summary>
		/// Item value has changed and needs to be re-sorted.
		/// </summary>
		/// <param name="item"></param>
		public void UpdateItem(T item)
		{
			SortUp(item);
		}

		/// <summary>
		/// Returns true if the item exists in the heap otherwise returns false.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			return Equals(_items[item.HeapIndex], item);
		}

		/// <summary>
		/// Sort the item down the heap.
		/// </summary>
		/// <param name="item"></param>
		private void SortDown(T item)
		{
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;

			if (childIndexLeft < _currentItemCount)
			{
				int swapIndex = childIndexLeft;
				if (childIndexRight < _currentItemCount)
				{
					if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) > 0)
					{
						swapIndex = childIndexRight;
					}
				}

				if (item.CompareTo(_items[swapIndex]) > 0)
				{
					Swap(item, _items[swapIndex]);
					SortDown(item);
				}
			}
		}

		/// <summary>
		/// Sort the item up the heap.
		/// </summary>
		/// <param name="item"></param>
		private void SortUp(T item)
		{
			int parentIndex = (item.HeapIndex - 1) / 2;
			T parentItem = _items[parentIndex];
			if (item.CompareTo(parentItem) < 0)
			{
				Swap(item, parentItem);
				SortUp(item);
			}
		}

		/// <summary>
		/// Swap two items in the heap.
		/// </summary>
		/// <param name="itemA"></param>
		/// <param name="itemB"></param>
		private void Swap(T itemA, T itemB)
		{
			_items[itemA.HeapIndex] = itemB;
			_items[itemB.HeapIndex] = itemA;
			(itemB.HeapIndex, itemA.HeapIndex) = (itemA.HeapIndex, itemB.HeapIndex);
		}
	}
}

#nullable disable
