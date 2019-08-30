using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridProjection
{
    public GameObject obj;
    public float t;

    public GridProjection(GameObject target, float time)
    {
        obj = target;
        t = time;
    }
}
