using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 12)]
public struct TilePos
{
    [FieldOffset(0)] public int x;
    [FieldOffset(4)] public int y;
    [FieldOffset(8)] public float height;

    public TilePos(int x, int y, float height)
    {
        this.x = x;
        this.y = y;
        this.height = height;
    }
}