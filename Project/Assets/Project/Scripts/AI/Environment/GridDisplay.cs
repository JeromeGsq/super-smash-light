using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridDisplay : MonoBehaviour
{
    public bool displayLockedCases;
    public bool displayProjections;
    public bool displayJSPData;

    GridMaker maker;

    private void OnDrawGizmos()
    {
        if(maker == null)
        {
            maker = GetComponent<GridMaker>();
        }
        GridCase[,] ourGrid = maker.GetGrid();


        for (int i = 0; i < ourGrid.GetLength(0); i++)
        {
            for (int j = 0; j < ourGrid.GetLength(1); j++)
            {

                if (displayLockedCases)
                {
                    Gizmos.color = Color.white;
                    if (ourGrid[i, j].isSolid)
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.1f, 0.1f, 0.1f));
                }


                if (displayProjections)
                {
                    Gizmos.color = Color.yellow;
                    if (ourGrid[i, j].projections.Count != 0)
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.5f, 0.5f, 0.5f));
                }

                if (displayJSPData)
                {
                    Gizmos.color = Color.red;
                    if (ourGrid[i, j].jumpPoints[0] || ourGrid[i, j].jumpPoints[1] || ourGrid[i, j].jumpPoints[2] || ourGrid[i, j].jumpPoints[3])
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.2f, 0.2f, 0.2f));

                    if (ourGrid[i, j].jumpPoints[0])
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2 - 0.25f, 0), new Vector3(0.15f, 0.15f, 0.15f));
                    if (ourGrid[i, j].jumpPoints[1])
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2 - 0.25f, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.15f, 0.15f, 0.15f));
                    if (ourGrid[i, j].jumpPoints[2])
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2 + 0.25f, 0), new Vector3(0.15f, 0.15f, 0.15f));
                    if (ourGrid[i, j].jumpPoints[3])
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2 + 0.25f, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.15f, 0.15f, 0.15f));
                }
            }
        }
    }
}
