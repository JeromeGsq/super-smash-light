using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCase
{
    public int myX, myY;
    public bool isSolid;
    public bool[] jumpPoints = new bool[8]; //N_NE_E_SE_S_SW_W_NW
    public int[] jumpDistances = new int[8]; //N_NE_E_SE_S_SW_W_NW

    public int[] parent = new int[2];
	public int givenCost = 0;
	public int finalCost = 0;
    public int parentDirection = 0;
	public ListStatus listStatus = ListStatus.ON_NONE;

    public bool hasTestStart, hasTestEnd, hasTestPath;

    public List<GridProjection> projections = new List<GridProjection>();

    public override string ToString()
    {
        string res = "Case " + myX + ":" + myY + " " + (isSolid ? "x\n":"\n");
        for (int i = 0; i < 8; i++) res += jumpPoints[i] ? 1 : 0;
        res += "\n";
        res += jumpDistances[7] + "\t"+ jumpDistances[0] + "\t" + jumpDistances[1] + "\n";
        res += jumpDistances[6] + "\t\t" + jumpDistances[2] + "\n";
        res += jumpDistances[5] + "\t" + jumpDistances[4] + "\t" + jumpDistances[3] + "\n";
        res += parent == null ? "No parent" : "Parent : " + parent[0] + ":" + parent[1] + ", " + parentDirection;
        return res;
    }
}

public enum ListStatus
{
    ON_NONE,
    ON_OPEN,
    ON_CLOSED
}
