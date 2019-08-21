using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCase
{
    public bool isSolid;
    public bool[] jumpPoints = new bool[4]; //NESW jump directions
    public int[] jumpDistances = new int[4]; //NESW distances
    public int[] diagonalDistances = new int[4]; //NE_SE_SW_NW

    public List<GridProjection> projections = new List<GridProjection>();
}
