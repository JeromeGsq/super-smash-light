using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TData : MonoBehaviour
{
    public int p1, p2, p3, p4;

    public override string ToString()
    {
        return "Saved data : " + p1 + " " + p2 + " " + p3 + " " + p4 + "\n";
    }
}
