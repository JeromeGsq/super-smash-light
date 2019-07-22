using UnityEngine;

public abstract class AgentAI
{
    public float horizontalSpeed;
    public float targetingX, targetingY;
    public bool jumpOn;
    public bool dashOn;
    public bool passOn;
    public bool shootOn;
    public bool diveOn;

    public void Update(GridCase[,] env, GameObject me)
    {
        jumpOn = false;
        dashOn = false;
        passOn = false;
        diveOn = false;

        UpdateAgent(env, me);
    }

    protected abstract void UpdateAgent(GridCase[,] env, GameObject me);
}