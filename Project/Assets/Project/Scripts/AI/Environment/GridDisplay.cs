using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridDisplay : MonoBehaviour
{
    public bool displayLockedCases;
    public bool displayProjections;
    public bool displayJPSData;
    public bool displayAgentData;

    GridMaker maker;
    public List<List<Vector3>> searchPaths = new List<List<Vector3>>();
    public List<Color> searchColors = new List<Color>();

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

                if (displayJPSData)
                {
                    Gizmos.color = Color.red;
                    if (ourGrid[i, j].jumpPoints[0] || ourGrid[i, j].jumpPoints[2] || ourGrid[i, j].jumpPoints[4] || ourGrid[i, j].jumpPoints[6])
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.2f, 0.2f, 0.2f));

                    if (ourGrid[i, j].jumpPoints[0])
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2 - 0.25f, 0), new Vector3(0.15f, 0.15f, 0.15f));
                    if (ourGrid[i, j].jumpPoints[2])
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2 - 0.25f, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.15f, 0.15f, 0.15f));
                    if (ourGrid[i, j].jumpPoints[4])
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2 + 0.25f, 0), new Vector3(0.15f, 0.15f, 0.15f));
                    if (ourGrid[i, j].jumpPoints[6])
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2 + 0.25f, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.15f, 0.15f, 0.15f));

                    Gizmos.color = Color.green;
                    if (ourGrid[i, j].hasTestStart)
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.6f, 0.6f, 0.6f));
                    Gizmos.color = Color.cyan;
                    if (ourGrid[i, j].hasTestEnd)
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.6f, 0.6f, 0.6f));
                    Gizmos.color = Color.white;
                    if (ourGrid[i, j].hasTestPath)
                        Gizmos.DrawCube(new Vector3(maker.startPos.x + i * maker.incrX + maker.incrX / 2, maker.startPos.y + j * maker.incrY + maker.incrY / 2, 0), new Vector3(0.4f, 0.4f, 0.4f));
                }
            }
        }

        if (displayAgentData)
        {
            foreach (var list in searchPaths)
            {
                if (list != null && list.Count >= 2)
                {
                    for (int k = 1; k < list.Count; k++)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(list[k - 1], list[k]);
                    }
                }
            }
        }
    }
}
