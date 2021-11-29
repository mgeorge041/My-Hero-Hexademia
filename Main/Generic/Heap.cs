using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinHeap<T> where T : IHeapItem<T>
{
    T[] items;
    int itemCount = 0;
    public int Count
    {
        get
        {
            return itemCount;
        }
    }

    // Constructor
    public MinHeap(int maxSize)
    {
        items = new T[maxSize];
    }

    // Print contents
    public void Print()
    {
        for (int i = 0; i < 10; i++)
        {
            try
            {
                Debug.Log(i + ": " + items[i].hexCoords + "> " + items[i].heapValue);
            }
            catch
            {
                Debug.Log(i + ": null");
            }
        }
    }

    // Add item to heap
    public void Add(T item)
    {
        // Do not add if at max size
        if (itemCount == items.Length)
        {
            throw new IndexOutOfRangeException();
        }

        item.heapIndex = itemCount;
        items[itemCount] = item;
        itemCount++;

        SortUp();
    }

    // Remove min item
    public T Pop()
    {
        if (itemCount == 0)
        {
            return default(T);
        }


        T root = items[0];
        items[0] = items[itemCount - 1];
        items[0].heapIndex = 0;
        itemCount--;
        SortDown();
        return root;
    }
    
    // Get whether item exists in heap
    public bool Contains(T item)
    {
        return Equals(items[item.heapIndex], item);
    }

    // Determine if item is at the root
    private bool IsRoot(int index)
    {
        if (index == 0)
        {
            return true;
        }
        return false;
    }

    // Swap two items
    private void Swap(int parentIndex, int childIndex)
    {
        // Update indices
        items[childIndex].heapIndex = parentIndex;
        items[parentIndex].heapIndex = childIndex;

        T temp = items[childIndex];
        items[childIndex] = items[parentIndex];
        items[parentIndex] = temp;
    }

    // Get right child
    private T GetRightChild(int index)
    {
        if ((index * 2 + 2) < itemCount)
            return items[index * 2 + 2];
        return default(T);
    }

    // Get left child
    private T GetLeftChild(int index)
    {
        if ((index * 2 + 1) < itemCount)
            return items[index * 2 + 1];
        return default(T);
    }

    // Get right child index
    private int GetRightChildIndex(int index)
    {
        return index * 2 + 2;
    }

    // Get left child index
    private int GetLeftChildIndex(int index)
    {
        return index * 2 + 1;
    }

    // Get whether has left child
    private bool HasLeftChild(int index)
    {
        if (GetLeftChild(index) != null)
        {
            return true;
        }
        return false;
    }

    // Get whether has right child
    private bool HasRightChild(int index)
    {
        if (GetRightChild(index) != null)
        {
            return true;
        }
        return false;
    }

    // Get parent of item
    private T GetParent(int index)
    {
        return items[GetParentIndex(index)];
    }

    // Get parent index
    private int GetParentIndex(int index)
    {
        return (index - 1) / 2;
    }

    // Sort up after adding a new item
    private void SortUp()
    {
        int index = itemCount - 1;
        while (!IsRoot(index) && items[index].heapValue < GetParent(index).heapValue)
        {
            int parentIndex = GetParentIndex(index);
            Swap(parentIndex, index);
            index = parentIndex;
        }
    }

    // Sort down after removing the root
    private void SortDown()
    {
        int index = 0;
        while (HasLeftChild(index))
        {
            int smallerItemIndex = GetLeftChildIndex(index);
            if (HasRightChild(index) && GetRightChild(index).heapValue < GetLeftChild(index).heapValue)
            {
                smallerItemIndex = GetRightChildIndex(index);
            }

            // End once gotten largest value to bottom
            if (items[smallerItemIndex].heapValue >= items[index].heapValue)
            {
                break;
            }

            Swap(index, smallerItemIndex);
            index = smallerItemIndex;
        }
    }
}
