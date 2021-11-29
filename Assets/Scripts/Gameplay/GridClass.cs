using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridClass
{
    //grid stuff
    private int height, width, depth;
    private float tileSize;
    public int[,] gridArray;

    public GridClass(int width, int depth, float tileSize)
    {
        this.width = width;
        this.depth = depth;
        this.tileSize = tileSize;

        gridArray = new int[width, depth];
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * tileSize;
    }
}
