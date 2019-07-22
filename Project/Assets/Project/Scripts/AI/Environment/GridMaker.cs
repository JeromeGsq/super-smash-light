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

    public GridCase[,] GetGrid()
    {
        return grid;
    }
}
