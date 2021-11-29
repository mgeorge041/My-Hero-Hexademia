using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHeapItem<T>
{
    int heapValue { get; }
    int heapIndex { get; set; }
    Vector3Int hexCoords { get; set; }
}
