using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridProjection
{
    private GameObject obj;
    private float t;

    public GridProjection(GameObject target, float time)
    {
        obj = target;
        t = time;
    }
}
