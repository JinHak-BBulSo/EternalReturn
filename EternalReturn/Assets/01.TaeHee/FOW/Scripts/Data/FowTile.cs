using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FowTile
{
    public TilePos pos;

    /// <summary> (x,y) 좌표, width를 이용해 계산한 일차원 배열 내 인덱스 </summary>
    public int index;

    public int X => pos.x;
    public int Y => pos.y;
    /// <summary> 해당 타일이 위치한 곳의 지형 높이 </summary>
    public float Height => pos.height;

    public FowTile(float height, int x, int y, int width)
    {
        pos.x = x;
        pos.y = y;
        pos.height = height;

        index = x + y * width;
    }
}