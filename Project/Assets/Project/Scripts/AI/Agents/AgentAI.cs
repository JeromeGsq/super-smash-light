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

    public void Update(GridMaker gm, GameObject me)
    {
        jumpOn = false;
        dashOn = false;
        diveOn = false;

        UpdateAgent(gm, me);
    }

    protected abstract void UpdateAgent(GridMaker gm, GameObject me);
}