using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionManager : MonoBehaviour
{
    private GameManagerView manager;

    public List<GameObject> entities;
    private List<Vector3> lastPositions = new List<Vector3>();

    public int numberProjections;
    public float projectionDelay;

    private bool hasLoaded = false;

    void Start()
    {
        manager = FindObjectOfType<GameManagerView>();
    }

    void Update()
    {
        if(!hasLoaded && manager.ProjectionReference1 != null)
        {
            entities.Add(manager.ProjectionReference1);
            entities.Add(manager.ProjectionReference2);
            entities.Add(manager.ProjectionReference3);
            entities.Add(manager.ProjectionReference4);
            lastPositions.Add(manager.ProjectionReference1.transform.position);
            lastPositions.Add(manager.ProjectionReference2.transform.position);
            lastPositions.Add(manager.ProjectionReference3.transform.position);
            lastPositions.Add(manager.ProjectionReference4.transform.position);
            hasLoaded = true;
        }

        if (hasLoaded)
        {

            for (int i = 0; i < GetComponent<GridMaker>().grid.GetLength(0); i++)
            {
                for (int j = 0; j < GetComponent<GridMaker>().grid.GetLength(1); j++)
                {
                    GetComponent<GridMaker>().grid[i, j].projections.Clear();
                }
            }

            for (int i = 0; i < entities.Count; i++)
            {
                float xSpeed = (entities[i].transform.position.x - lastPositions[i].x) / Time.deltaTime;
                float ySpeed = (entities[i].transform.position.y - lastPositions[i].y) / Time.deltaTime;

                lastPositions[i] = entities[i].transform.position;

                Vector3 newPos = entities[i].transform.position;
                float gravity = -60 * projectionDelay;

                for (int j = 0; j < numberProjections; j++)
                {

                    newPos += new Vector3(xSpeed * projectionDelay, ySpeed * projectionDelay, 0);

                    int xIndex = (int)((newPos.x - GetComponent<GridMaker>().startPos.x) / GetComponent<GridMaker>().incrX);
                    int yIndex = (int)((newPos.y - GetComponent<GridMaker>().startPos.y) / GetComponent<GridMaker>().incrY);

                    if (xIndex >= 0 && xIndex < GetComponent<GridMaker>().grid.GetLength(0) && 
                        yIndex >= 0 && yIndex < GetComponent<GridMaker>().grid.GetLength(1) && 
                        !GetComponent<GridMaker>().grid[xIndex, yIndex].isSolid)
                    {
                        GetComponent<GridMaker>().grid[xIndex, yIndex].projections.Add(new GridProjection(entities[i], j * projectionDelay));
                    }
                    else break;
                    if(ySpeed != 0) ySpeed += gravity;
                }
            }
        }
    }
}
