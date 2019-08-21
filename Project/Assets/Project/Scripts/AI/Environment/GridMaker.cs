using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GridMaker : MonoBehaviour
{
    public GameObject wallParent;
    public GameObject platformParent;

    public Vector3 startPos, endPos;
    public SpriteRenderer startPosIndicator, endPosIndicator;
    public float incrX = 1f;
    public float incrY = -1f;

    private Vector3 o_startPos, o_endPos;

    internal GridCase[,] grid;

    void Start()
    {
        Debug.Log("Pos : " + startPos.ToString() + " " + endPos.ToString() + "\n" + (endPos.x - startPos.x) + " : " + (endPos.y - startPos.y));
        startPosIndicator.enabled = false;
        endPosIndicator.enabled = false;
    }

    void Update()
    {
        if (startPosIndicator.transform.position != startPos) startPosIndicator.transform.position = startPos;
        if (endPosIndicator.transform.position != endPos) endPosIndicator.transform.position = endPos;

        if (startPos != o_startPos || endPos != o_endPos || grid == null)
        {
            o_startPos = startPos;
            o_endPos = endPos;

            grid = new GridCase[(int)Mathf.Abs((endPos.x - startPos.x) * (1/incrX)), (int)Mathf.Abs((endPos.y - startPos.y) * (1/incrY))];

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new GridCase();

                    Vector3 localPos = new Vector3(startPos.x + i * incrX + incrX / 2, startPos.y + j * incrY + incrY / 2, 0);
                    foreach (BoxCollider2D collider in wallParent.GetComponentsInChildren<BoxCollider2D>())
                    {
                        if(collider.bounds.Contains(localPos)) grid[i, j].isSolid = true;
                    }
                    foreach (BoxCollider2D collider in platformParent.GetComponentsInChildren<BoxCollider2D>())
                    {
                        if (collider.bounds.Contains(localPos)) grid[i, j].isSolid = true;
                    }
                }
            }
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
                    grid[i, j].jumpPoints[1] = true;
                    grid[i, j].jumpPoints[2] = true;
                }
                if (i < grid.GetLength(0) - 1 && j > 0 && !grid[i, j].isSolid && !grid[i + 1, j].isSolid && !grid[i, j - 1].isSolid && grid[i + 1, j - 1].isSolid)
                {
                    grid[i, j].jumpPoints[2] = true;
                    grid[i, j].jumpPoints[3] = true;
                }
                if (i < grid.GetLength(0) - 1 && j < grid.GetLength(1) - 1 && !grid[i, j].isSolid && !grid[i + 1, j].isSolid && !grid[i, j + 1].isSolid && grid[i + 1, j + 1].isSolid)
                {
                    grid[i, j].jumpPoints[3] = true;
                    grid[i, j].jumpPoints[0] = true;
                }
                if (i > 0 && j < grid.GetLength(1) - 1 && !grid[i, j].isSolid && !grid[i - 1, j].isSolid && !grid[i, j + 1].isSolid && grid[i - 1, j + 1].isSolid)
                {
                    grid[i, j].jumpPoints[0] = true;
                    grid[i, j].jumpPoints[1] = true;
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
                CheckGrid(i, j, 2, score, foundJP);
            }
            score = 0;
            foundJP = false;

            for (int j = grid.GetLength(1) - 1; j >= 0; j--)
            {
                CheckGrid(i, j, 0, score, foundJP);
            }
            score = 0;
            foundJP = false;
        }
        for (int j = 0; j < grid.GetLength(1); j++)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                CheckGrid(i, j, 1, score, foundJP);
            }
            score = 0;
            foundJP = false;

            for (int i = grid.GetLength(0) - 1; i >= 0; i--)
            {
                CheckGrid(i, j, 3, score, foundJP);
            }
            score = 0;
            foundJP = false;
        }
    }

    private void CheckGrid(int i, int j, int direction, int score, bool foundJumpPoint)
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
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (!grid[i, j].isSolid)
                {
                    if (i == 0 || j == 0 || grid[i - 1, j].isSolid || grid[i, j - 1].isSolid || grid[i - 1, j - 1].isSolid)
                    {
                        grid[i, j].diagonalDistances[3] = 0;
                    }
                    else if (grid[i - 1, j - 1].jumpDistances[0] > 1 || grid[i - 1, j - 1].jumpDistances[3] > 1)
                    {
                        grid[i, j].diagonalDistances[3] = 1;
                    }
                    else
                    {
                        grid[i, j].diagonalDistances[3] = grid[i - 1, j - 1].diagonalDistances[3] + grid[i - 1, j - 1].diagonalDistances[3] > 0? 1:-1;
                    }
                }
            }

            for (int j = grid.GetLength(1) - 1; j >= 0; j--)
            {
                if (!grid[i, j].isSolid)
                {
                    if (i == 0 || j == grid.GetLength(1) - 1 || grid[i - 1, j].isSolid || grid[i, j + 1].isSolid || grid[i - 1, j + 1].isSolid)
                    {
                        grid[i, j].diagonalDistances[2] = 0;
                    }
                    else if (grid[i - 1, j + 1].jumpDistances[2] > 1 || grid[i - 1, j + 1].jumpDistances[3] > 1)
                    {
                        grid[i, j].diagonalDistances[2] = 1;
                    }
                    else
                    {
                        grid[i, j].diagonalDistances[2] = grid[i - 1, j + 1].diagonalDistances[2] + grid[i - 1, j + 1].diagonalDistances[2] > 0 ? 1 : -1;
                    }
                }
            }
        }

        for (int i = grid.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (!grid[i, j].isSolid)
                {
                    if (i == grid.GetLength(0) - 1 || j == 0 || grid[i + 1, j].isSolid || grid[i, j - 1].isSolid || grid[i + 1, j - 1].isSolid)
                    {
                        grid[i, j].diagonalDistances[0] = 0;
                    }
                    else if (grid[i + 1, j - 1].jumpDistances[0] > 1 || grid[i + 1, j - 1].jumpDistances[1] > 1)
                    {
                        grid[i, j].diagonalDistances[0] = 1;
                    }
                    else
                    {
                        grid[i, j].diagonalDistances[0] = grid[i + 1, j - 1].diagonalDistances[0] + grid[i + 1, j - 1].diagonalDistances[0] > 0 ? 1 : -1;
                    }
                }
            }


            for (int j = grid.GetLength(1) - 1; j >= 0; j--)
            {
                if (!grid[i, j].isSolid)
                {
                    if (i == grid.GetLength(0) - 1 || j == grid.GetLength(1) - 1 || grid[i + 1, j].isSolid || grid[i, j + 1].isSolid || grid[i + 1, j + 1].isSolid)
                    {
                        grid[i, j].diagonalDistances[1] = 0;
                    }
                    else if (grid[i + 1, j + 1].jumpDistances[2] > 1 || grid[i + 1, j + 1].jumpDistances[1] > 1)
                    {
                        grid[i, j].diagonalDistances[1] = 1;
                    }
                    else
                    {
                        grid[i, j].diagonalDistances[1] = grid[i + 1, j + 1].diagonalDistances[1] + grid[i + 1, j + 1].diagonalDistances[1] > 0 ? 1 : -1;
                    }
                }
            }
        }
    }

    public GridCase[,] GetGrid()
    {
        return grid;
    }
}
