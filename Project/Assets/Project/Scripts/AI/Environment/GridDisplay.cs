using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridDisplay : MonoBehaviour
{
    public bool displayLockedCases;
    public bool displayProjections;

    private void OnDrawGizmos()
    {
        if(displayLockedCases)
        {
            Gizmos.color = Color.white;
            GridMaker maker = GetComponent<GridMaker>();
            GridCase[,] ourGrid = maker.GetGrid();

            for(int i = 0;i< ourGrid.GetLength(0);i++)
            {
                for(int j = 0; j<ourGrid.GetLength(1);j++)
                {
                    if (ourGrid[i, j].isSolid)
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0),new Vector3(0.1f,0.1f,0.1f));
                }
            }
        }

        if (displayProjections)
        {
            Gizmos.color = Color.yellow;
            GridMaker maker = GetComponent<GridMaker>();
            GridCase[,] ourGrid = maker.GetGrid();

            for (int i = 0; i < ourGrid.GetLength(0); i++)
            {
                for (int j = 0; j < ourGrid.GetLength(1); j++)
                {
                    if (ourGrid[i, j].projections.Count != 0)
                    {
                        foreach(GridProjection g in ourGrid[i, j].projections)
                        {
                            Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.5f, 0.5f, 0.5f));
                        }
                    }
                }
            }
        }
    }
}
