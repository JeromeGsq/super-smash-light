using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GridMaker : MonoBehaviour
{
    public GameObject wallParent;
    public GameObject platformParent;

    public Vector3 startPos, endPos, test_startPos, test_endPos;
    public GameObject startPosIndicator, endPosIndicator, testStartIndicator, testEndIndicator;
    public float incrX = 1f;
    public float incrY = -1f;

    private Vector3 o_startPos, o_endPos, o_test_start, o_test_end;

    internal GridCase[,] grid;

    static readonly float SQRT_2 = Mathf.Sqrt(2);
    static readonly float SQRT_2_MINUS_1 = Mathf.Sqrt(2) - 1.0f;

    void Start()
    {
        Debug.Log("Pos : " + startPos.ToString() + " " + endPos.ToString() + "\n" + (endPos.x - startPos.x) + " : " + (endPos.y - startPos.y));
    }

    void Update()
    {
        if (startPosIndicator.transform.position != startPos) startPos = startPosIndicator.transform.position;
        if (endPosIndicator.transform.position != endPos) endPos = endPosIndicator.transform.position;

        if (testStartIndicator.transform.position != test_startPos) test_startPos = testStartIndicator.transform.position;
        if (testEndIndicator.transform.position != test_endPos) test_endPos = testEndIndicator.transform.position;

        if (startPos != o_startPos || endPos != o_endPos || grid == null)
        {
            o_startPos = startPos;
            o_endPos = endPos;

            grid = new GridCase[(int)Mathf.Abs((endPos.x - startPos.x) * (1 / incrX)), (int)Mathf.Abs((endPos.y - startPos.y) * (1 / incrY))];

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new GridCase();
                    grid[i, j].myX = i;
                    grid[i, j].myY = j;

                    Vector3 localPos = new Vector3(startPos.x + i * incrX + incrX / 2, startPos.y + j * incrY + incrY / 2, 0);
                    foreach (BoxCollider2D collider in wallParent.GetComponentsInChildren<BoxCollider2D>())
                    {
                        if (collider.bounds.Contains(localPos)) grid[i, j].isSolid = true;
                    }
                    foreach (BoxCollider2D collider in platformParent.GetComponentsInChildren<BoxCollider2D>())
                    {
                        if (collider.bounds.Contains(localPos)) grid[i, j].isSolid = true;
                    }
                }
            }
        }

        if ((test_startPos != o_test_start || test_endPos != o_test_end) && grid!=null)
        {
            o_test_start = test_startPos;
            o_test_end = test_endPos;

            int localsX = (int)((o_test_start.x - o_startPos.x) / incrX);
            int localsY = (int)((o_test_start.y - o_startPos.y) / incrY);

            int localeX = (int)((o_test_end.x - o_startPos.x) / incrX);
            int localeY = (int)((o_test_end.y - o_startPos.y) / incrY);

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].hasTestStart = false;
                    grid[i, j].hasTestEnd = false;
                }
            }

            grid[localsX, localsY].hasTestStart = true;
            grid[localeX, localeY].hasTestEnd = true;

            JSPTest();
            //Debug.Log(grid[localeX, localeY].ToString());
        }
    }

    public void BuildJPSData()
    {
        CalcPrimaryJumpPoints();
        CalcJumpDistances();
        CalcDiagonalDistances();
    }

    private void CalcPrimaryJumpPoints()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (i > 0 && j > 0 && !grid[i, j].isSolid && !grid[i - 1, j].isSolid && !grid[i, j - 1].isSolid && grid[i - 1, j - 1].isSolid)
                {
                    grid[i, j].jumpPoints[2] = true;
                    grid[i, j].jumpPoints[4] = true;
                }
                if (i < grid.GetLength(0) - 1 && j > 0 && !grid[i, j].isSolid && !grid[i + 1, j].isSolid && !grid[i, j - 1].isSolid && grid[i + 1, j - 1].isSolid)
                {
                    grid[i, j].jumpPoints[4] = true;
                    grid[i, j].jumpPoints[6] = true;
                }
                if (i < grid.GetLength(0) - 1 && j < grid.GetLength(1) - 1 && !grid[i, j].isSolid && !grid[i + 1, j].isSolid && !grid[i, j + 1].isSolid && grid[i + 1, j + 1].isSolid)
                {
                    grid[i, j].jumpPoints[6] = true;
                    grid[i, j].jumpPoints[0] = true;
                }
                if (i > 0 && j < grid.GetLength(1) - 1 && !grid[i, j].isSolid && !grid[i - 1, j].isSolid && !grid[i, j + 1].isSolid && grid[i - 1, j + 1].isSolid)
                {
                    grid[i, j].jumpPoints[0] = true;
                    grid[i, j].jumpPoints[2] = true;
                }
            }
        }
    }

    private void CalcJumpDistances()
    {
        int score = 0;
        bool foundJP = false;

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                CheckGrid(i, j, 0, ref score, ref foundJP);
            }
            score = 0;
            foundJP = false;

            for (int j = grid.GetLength(1) - 1; j >= 0; j--)
            {
                CheckGrid(i, j, 4, ref score, ref foundJP);
            }
            score = 0;
            foundJP = false;
        }
        for (int j = 0; j < grid.GetLength(1); j++)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                CheckGrid(i, j, 6, ref score, ref foundJP);
            }
            score = 0;
            foundJP = false;

            for (int i = grid.GetLength(0) - 1; i >= 0; i--)
            {
                CheckGrid(i, j, 2, ref score, ref foundJP);
            }
            score = 0;
            foundJP = false;
        }
    }

    private void CheckGrid(int i, int j, int direction, ref int score, ref bool foundJumpPoint)
    {
        if (grid[i, j].isSolid)
        {
            score = 0;
            foundJumpPoint = false;
        }
        else
        {
            if (foundJumpPoint) grid[i, j].jumpDistances[direction] = score;
            else grid[i, j].jumpDistances[direction] = -score;

            score++;

            if (grid[i, j].jumpPoints[direction])
            {
                score = 1;
                foundJumpPoint = true;
            }
        }

    }

    private void CalcDiagonalDistances()
    {
        for (int j = 0; j < grid.GetLength(1); ++j)
        {
            for (int i = 0; i < grid.GetLength(0); ++i)
            {
                if (grid[i,j].isSolid) continue;

                // Calculate NORTH WEST DISTNACES
                if (j == 0 || i == 0 || (                  // If we in the north west corner
                    grid[i,j-1].isSolid ||          // If the node to the north is an obstacle
                    grid[i-1,j].isSolid ||           // If the node to the left is an obstacle
                    grid[i-1,j-1].isSolid))     // if the node to the North west is an obstacle
                {
                    grid[i,j].jumpDistances[7] = 0;
                }
                else if (!grid[i,j-1].isSolid &&                                                    // if the node to the north is empty
                    !grid[i-1,j].isSolid &&                                                          // if the node to the west is empty
                    (grid[i - 1,j - 1].jumpDistances[0] > 0 ||    // If the node to the north west has is a straight jump point ( or primary jump point) going north
                     grid[i - 1, j - 1].jumpDistances[6] > 0))     // If the node to the north west has is a straight jump point ( or primary jump point) going West
                {
                    // Diagonal one away
                    grid[i,j].jumpDistances[7] = 1;
                }
                else
                {
                    // Increment from last
                    int jumpDistance = grid[i - 1,j - 1].jumpDistances[7];

                    if (jumpDistance > 0)
                    {
                        grid[i,j].jumpDistances[7] = 1 + jumpDistance;
                    }
                    else //if( jumpDistance <= 0 )
                    {
                        grid[i,j].jumpDistances[7] = -1 + jumpDistance;
                    }
                }

                // Calculate NORTH EAST DISTNACES
                if (j == 0 || i == grid.GetLength(0) - 1 || (         // If we in the top right corner
                    grid[i,j-1].isSolid ||          // If the node to the north is an obstacle
                    grid[i+1,j].isSolid) ||           // If the node to the east is an obstacle
                    grid[i+1,j-1].isSolid)     // if the node to the North East is an obstacle
                {
                    // Wall one away
                    grid[i,j].jumpDistances[1] = 0;
                }
                else if (!grid[i,j-1].isSolid &&                                                    // if the node to the north is empty
                    !grid[i+1,j].isSolid &&                                                          // if the node to the east is empty
                    (grid[i + 1,j - 1].jumpDistances[0] > 0 ||    // If the node to the north east has is a straight jump point ( or primary jump point) going north
                     grid[i + 1,j - 1].jumpDistances[2] > 0))     // If the node to the north east has is a straight jump point ( or primary jump point) going east
                {
                    // Diagonal one away
                    grid[i,j].jumpDistances[1] = 1;
                }
                else
                {
                    // Increment from last
                    int jumpDistance = grid[i + 1,j - 1].jumpDistances[1];

                    if (jumpDistance > 0)
                    {
                        grid[i,j].jumpDistances[1] = 1 + jumpDistance;
                    }
                    else //if( jumpDistance <= 0 )
                    {
                        grid[i,j].jumpDistances[1] = -1 + jumpDistance;
                    }
                }
            }
        }

        // Calcin' Jump Distance, Diagonally DownLeft and Downright
        // For all the rows in the grid
        for (int j = grid.GetLength(1) - 1; j >= 0; --j)
        {
            for (int i = 0; i < grid.GetLength(0); ++i)
            {
                if (grid[i,j].isSolid) continue;

                // Calculate SOUTH WEST DISTNACES
                if (j == grid.GetLength(1) - 1 || i == 0 || (         // If we in the south west most node
                    grid[i,j+1].isSolid ||          // If the node to the south is an obstacle
                    grid[i-1,j].isSolid ||           // If the node to the west is an obstacle
                    grid[i - 1,j+1].isSolid))     // if the node to the south West is an obstacle
                {
                    // Wall one away
                    grid[i,j].jumpDistances[5] = 0;
                }
                else if (!grid[i,j+1].isSolid &&                                                    // if the node to the south is empty
                    !grid[i-1,j].isSolid &&                                                          // if the node to the west is empty
                    (grid[i - 1,j + 1].jumpDistances[4] > 0 ||    // If the node to the south west has is a straight jump point ( or primary jump point) going south
                     grid[i - 1,j + 1].jumpDistances[6] > 0))     // If the node to the south west has is a straight jump point ( or primary jump point) going West
                {
                    // Diagonal one away
                    grid[i,j].jumpDistances[5] = 1;
                }
                else
                {
                    // Increment from last
                    int jumpDistance = grid[i - 1,j + 1].jumpDistances[5];

                    if (jumpDistance > 0)
                    {
                        grid[i,j].jumpDistances[5] = 1 + jumpDistance;
                    }
                    else //if( jumpDistance <= 0 )
                    {
                        grid[i,j].jumpDistances[5] = -1 + jumpDistance;
                    }
                }

                // Calculate SOUTH EAST DISTNACES
                if (j == grid.GetLength(1) - 1 || i == grid.GetLength(0) - 1 || (    // If we in the south east corner
                    grid[i,j+1].isSolid ||               // If the node to the south is an obstacle
                    grid[i+1,j].isSolid ||                // If the node to the east is an obstacle
                    grid[i+1,j+1].isSolid))          // if the node to the south east is an obstacle
                {
                    // Wall one away
                    grid[i,j].jumpDistances[3] = 0;
                }
                else if (!grid[i,j+1].isSolid &&                                                    // if the node to the south is empty
                    !grid[i+1,j].isSolid &&                                                          // if the node to the east is empty
                    (grid[i + 1,j + 1].jumpDistances[4] > 0 ||    // If the node to the south east has is a straight jump point ( or primary jump point) going south
                     grid[i + 1,j + 1].jumpDistances[2] > 0))     // If the node to the south east has is a straight jump point ( or primary jump point) going east
                {
                    // Diagonal one away
                    grid[i,j].jumpDistances[3] = 1;
                }
                else
                {
                    // Increment from last
                    int jumpDistance = grid[i + 1,j + 1].jumpDistances[3];

                    if (jumpDistance > 0)
                    {
                        grid[i,j].jumpDistances[3] = 1 + jumpDistance;
                    }
                    else //if( jumpDistance <= 0 )
                    {
                        grid[i,j].jumpDistances[3] = -1 + jumpDistance;
                    }
                }
            }
        }
    }

    public List<int[]> BuildPath(int sx, int sy, int ex, int ey)
    {
        PriorityQueue<int[],int> open_set = new PriorityQueue<int[], int>();

        ResetPathData();

        int testcount = 100;

        grid[sx,sy].listStatus = ListStatus.ON_OPEN;
        open_set.push(new int[] { sx, sy }, 0);

        while (!open_set.isEmpty() && testcount > 0)
        {
            testcount--;
            int[] curr_node = open_set.pop();
            int[] parent = grid[curr_node[0],curr_node[1]].parent;

            //Debug.Log(grid[curr_node[0], curr_node[1]].ToString());

            if (curr_node[0] == ex && curr_node[1] == ey)
            {
                //Debug.Log("PATH FOUND");
                List<int[]> path = new List<int[]>();

                while (grid[curr_node[0], curr_node[1]].parent != null)
                {
                    path.Add(new int[] { curr_node[0], curr_node[1] });
                    curr_node = grid[curr_node[0], curr_node[1]].parent;
                }
                path.Add(new int[] { sx, sy });

                path.Reverse();
                return path;
            }

            int[] directions = grid[curr_node[0], curr_node[1]].parent == null ?
                new int[] { 0, 1, 2, 3, 4, 5, 6, 7 } :
                projectedDirections(grid[curr_node[0], curr_node[1]].parentDirection);

            foreach (int dir in directions)
            {
                //Debug.Log("testing " + dir);
                int[] new_successor = null;
                int given_cost = 0;

                bool isInCardinalDirection = dir % 2 == 0 && towardsDirection(ex - curr_node[0], ey - curr_node[1], dir, true);
                bool isInDiagonalDirection = dir % 2 == 1 && towardsDirection(ex - curr_node[0], ey - curr_node[1], dir, false);

                bool jumpDistanceIsFurtherThanXDiff = Math.Abs(ex - curr_node[0]) <= Mathf.Abs(grid[curr_node[0], curr_node[1]].jumpDistances[dir]);
                bool jumpDustanceIsFurtherThanYDiff = Math.Abs(ey - curr_node[1]) <= Mathf.Abs(grid[curr_node[0], curr_node[1]].jumpDistances[dir]);

                if (isInCardinalDirection && jumpDistanceIsFurtherThanXDiff && jumpDustanceIsFurtherThanYDiff)
                {
                    //Debug.Log("chech 1 " + dir);
                    new_successor = new int[] { ex, ey };
                    given_cost = grid[curr_node[0], curr_node[1]].givenCost + Math.Max(Math.Abs(ex - curr_node[0]), Math.Abs(ey - curr_node[1]));
                }

                else if (isInDiagonalDirection && (jumpDistanceIsFurtherThanXDiff || jumpDustanceIsFurtherThanYDiff))
                {
                    //Debug.Log("chech 2 " + dir);
                    int min_diff = Mathf.Min(Mathf.Abs(ex - curr_node[0]), Mathf.Abs(ey - curr_node[1]));
                    new_successor = projectDistance(curr_node[0], curr_node[1], dir, min_diff);
                    given_cost = grid[curr_node[0], curr_node[1]].givenCost + (int)(SQRT_2 * Math.Max(Math.Abs(new_successor[0] - curr_node[0]), Math.Abs(new_successor[1] - curr_node[1])));
                }
                else if (grid[curr_node[0], curr_node[1]].jumpDistances[dir] > 0)
                {
                    //Debug.Log("chech 3 " + dir);
                    new_successor = projectDistance(curr_node[0], curr_node[1], dir, grid[curr_node[0], curr_node[1]].jumpDistances[dir]);
                    given_cost = Math.Max(Math.Abs(new_successor[0] - curr_node[0]), Math.Abs(new_successor[1] - curr_node[1]));
                    if (dir % 2 == 1) given_cost = (int)(given_cost * SQRT_2);
                    given_cost += grid[curr_node[0], curr_node[1]].givenCost;
                }

                if (new_successor != null)
                {
                    if (grid[new_successor[0], new_successor[1]].listStatus != ListStatus.ON_OPEN)
                    {
                        grid[new_successor[0], new_successor[1]].parent = curr_node;
                        grid[new_successor[0], new_successor[1]].givenCost = given_cost;
                        grid[new_successor[0], new_successor[1]].parentDirection = dir;
                        grid[new_successor[0], new_successor[1]].finalCost = given_cost + octileHeuristic(new_successor[0], new_successor[1], ex, ey);
                        grid[new_successor[0], new_successor[1]].listStatus = ListStatus.ON_OPEN;
                        open_set.push(new_successor, grid[new_successor[0], new_successor[1]].finalCost);
                    }
                    else if (given_cost < grid[new_successor[0], new_successor[1]].givenCost)
                    {
                        grid[new_successor[0], new_successor[1]].parent = curr_node;
                        grid[new_successor[0], new_successor[1]].givenCost = given_cost;
                        grid[new_successor[0], new_successor[1]].parentDirection = dir;
                        grid[new_successor[0], new_successor[1]].finalCost = given_cost + octileHeuristic(new_successor[0], new_successor[1], ex, ey);
                        grid[new_successor[0], new_successor[1]].listStatus = ListStatus.ON_OPEN;
                        open_set.push(new_successor, grid[new_successor[0], new_successor[1]].finalCost);
                    }
                }
            }
        }

        return new List<int[]>();
    }

    public void JSPTest()
    {
        int[] start = new int[2];
        int[] end = new int[2];

        for(int i = 0; i<grid.GetLength(0);i++)
        {
            for(int j = 0;j<grid.GetLength(1);j++)
            {
                if(grid[i,j].hasTestStart)
                {
                    start[0] = i;
                    start[1] = j;
                }
                if(grid[i,j].hasTestEnd)
                {
                    end[0] = i;
                    end[1] = j;
                }
                grid[i, j].hasTestPath = false;
            }
        }

        var res = BuildPath(start[0], start[1], end[0], end[1]);
        //Debug.Log(res.Count + " " + start[0] + " " + start[1] + " " + end[0] + " " + end[1]);
        foreach(var g in res)
        {
            grid[g[0], g[1]].hasTestPath = true;
        }
    }

    internal static int octileHeuristic(int curr_row, int curr_column, int goal_row, int goal_column)
    {
        int heuristic;
        int row_dist = goal_row - curr_row;
        int column_dist = goal_column - curr_column;

        heuristic = (int)(Mathf.Max(row_dist, column_dist) + SQRT_2_MINUS_1 * Mathf.Min(row_dist, column_dist));

        return heuristic;
    }

    private int[] projectDistance(int sx, int sy, int dir, int dist)
    {
        switch (dir)
        {
            case 0:
                return new int[] { sx, sy - dist };
            case 1:
                return new int[] { sx + dist, sy - dist };
            case 2:
                return new int[] { sx + dist, sy };
            case 3:
                return new int[] { sx + dist, sy + dist };
            case 4:
                return new int[] { sx, sy + dist };
            case 5:
                return new int[] { sx - dist, sy + dist };
            case 6:
                return new int[] { sx - dist, sy };
            case 7:
                return new int[] { sx - dist, sy - dist };
            default:
                return new int[0];
        }
    }

    private int[] projectedDirections(int dir)
    {
        switch(dir)
        {
            case 0:
                return new int[] { 6, 7, 0, 1, 2 };
            case 1:
                return new int[] { 0, 1, 2 };
            case 2:
                return new int[] { 0, 1, 2, 3, 4 };
            case 3:
                return new int[] { 2, 3, 4 };
            case 4:
                return new int[] { 2, 3, 4, 5, 6 };
            case 5:
                return new int[] { 4, 5, 6 };
            case 6:
                return new int[] { 4, 5, 6, 7, 0 };
            case 7:
                return new int[] { 6, 7, 0 };
            default:
                return new int[0];
        }
    }

    private bool towardsDirection(int xdif, int ydif, int dir, bool isExact)
    {
        switch (dir)
        {
            case 0:
                return xdif == 0 && ydif > 0;
            case 1:
                return xdif > 0 && ydif < 0 && (isExact? xdif * xdif == ydif * ydif : true);
            case 2:
                return xdif > 0 && ydif == 0;
            case 3:
                return xdif > 0 && ydif > 0 && (isExact ? xdif * xdif == ydif * ydif : true);
            case 4:
                return xdif == 0 && ydif < 0;
            case 5:
                return xdif < 0 && ydif > 0 && (isExact ? xdif * xdif == ydif * ydif : true);
            case 6:
                return xdif < 0 && ydif == 0;
            case 7:
                return xdif < 0 && ydif < 0 && (isExact ? xdif * xdif == ydif * ydif : true);
            default:
                return false;
        }
    }

    private void ResetPathData()
    {
        for(int i = 0; i < grid.GetLength(0);i++)
        {
            for(int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j].parent = null;
                grid[i, j].parentDirection = -1;
                grid[i, j].givenCost = 0;
                grid[i, j].finalCost = 0;
                grid[i, j].listStatus = ListStatus.ON_NONE;
            }
        }
    }

    public GridCase[,] GetGrid()
    {
        return grid;
    }
}
