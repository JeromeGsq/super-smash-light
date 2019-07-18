using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class ControllerEmulator : MonoBehaviour
{
    public bool LeftTrigger { get; internal set; }
    public bool RightTrigger { get; internal set; }
    public bool LeftShoulder { get; internal set; }
    public bool RightShoulder { get; internal set; }
    public bool ButtonA { get; internal set; }
    public bool ButtonB { get; internal set; }

    public Vector2 RightThumbstick { get; internal set; }
    public Vector2 LeftThumbstick { get; internal set; }

    private float LT, RT, A, B, m;
    private bool movementTendency;

    void Start()
    {
        LT = Time.realtimeSinceStartup + 1 + 3*Random.value;
        RT = Time.realtimeSinceStartup + 1 + 3 * Random.value;
        A = Time.realtimeSinceStartup + Random.value;
        B = Time.realtimeSinceStartup + 1 + 3 * Random.value;
        m = Time.realtimeSinceStartup + 3 * Random.value;

        RightThumbstick = new Vector2();
        LeftThumbstick = new Vector2();
    }

    void Update()
    {
        ButtonA = false;
        ButtonB = false;

        RightThumbstick += new Vector2 ((Random.value - 0.5f) * Random.value, (Random.value - 0.5f) * Random.value);
        LeftThumbstick += new Vector2((Random.value - 0.5f) * Random.value, 0);
        if (movementTendency) LeftThumbstick += new Vector2(0.5f, 0);
        else LeftThumbstick += new Vector2(-0.5f, 0);

        if (Time.realtimeSinceStartup > LT)
        {
            LT = Time.realtimeSinceStartup + 1 + 3 * Random.value;
            LeftTrigger = !LeftTrigger;
        }
        if (Time.realtimeSinceStartup > RT)
        {
            RT = Time.realtimeSinceStartup + 1 + 3 * Random.value;
            RightTrigger = !RightTrigger;
        }
        if (Time.realtimeSinceStartup > A)
        {
            A = Time.realtimeSinceStartup + Random.value;
            ButtonA = true;
        }
        if (Time.realtimeSinceStartup > B)
        {
            B = Time.realtimeSinceStartup + 1 + Random.value;
            ButtonB = true;
        }
        if (Time.realtimeSinceStartup > m)
        {
            m = Time.realtimeSinceStartup + 3 * Random.value;
            movementTendency = !movementTendency;
        }
    }
}
