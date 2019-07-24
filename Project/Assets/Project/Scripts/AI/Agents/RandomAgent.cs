using UnityEngine;

public class RandomAgent : AgentAI
{
    private float LT, RT, A, B, m;
    private bool movementTendency;

    public RandomAgent()
    {
        LT = Time.realtimeSinceStartup + 1 + 3 * Random.value;
        RT = Time.realtimeSinceStartup + 1 + 3 * Random.value;
        A = Time.realtimeSinceStartup + Random.value;
        B = Time.realtimeSinceStartup + Random.value;
        m = Time.realtimeSinceStartup + 3 * Random.value;
    }

    protected override void UpdateAgent(GridCase[,] env, GameObject me)
    {
        horizontalSpeed += 0.1f * (Random.value - 0.5f);
        if (Mathf.Abs(me.transform.position.x) > 10) horizontalSpeed -= me.transform.position.x / 30;
        if (movementTendency) horizontalSpeed -= 0.4f;
        else horizontalSpeed += 0.4f;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -1, 1);

        targetingX = (Random.value - 0.5f) * Random.value;
        targetingY = (Random.value - 0.5f) * Random.value;


        if (Time.realtimeSinceStartup > LT)
        {
            LT = Time.realtimeSinceStartup + 1 + 3 * Random.value;
            shootOn = !shootOn;
        }
        if (Time.realtimeSinceStartup > RT)
        {
            RT = Time.realtimeSinceStartup + 1 + 3 * Random.value;
            passOn = !passOn;
        }
        if (Time.realtimeSinceStartup > A)
        {
            A = Time.realtimeSinceStartup + Random.value;
            jumpOn = true;
        }
        if (Time.realtimeSinceStartup > B)
        {
            B = Time.realtimeSinceStartup + Random.value;
            dashOn = true;
        }
        if (Time.realtimeSinceStartup > m)
        {
            m = Time.realtimeSinceStartup + 3 * Random.value;
            movementTendency = !movementTendency;
        }
    }

}