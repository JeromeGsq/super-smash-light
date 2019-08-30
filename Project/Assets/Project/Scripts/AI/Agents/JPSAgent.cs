using UnityEngine;
using System.Collections.Generic;

public class JPSAgent : AgentAI
{
    public List<Vector3> pathFriend = new List<Vector3>();
    public List<Vector3> pathBall = new List<Vector3>();
    public List<Vector3> pathFoe1 = new List<Vector3>();
    public List<Vector3> pathFoe2 = new List<Vector3>();

    public BallHandler ballRef;

    public Vector3 lastPosition;
    
    public JPSAgent()
    {

    }

    protected override void UpdateAgent(GridMaker gm, GameObject me)
    {
        if (ballRef == null) ballRef = gm.view.BallProjectionReference.GetComponent<BallHandler>();


        int[] self = null;
        int[] friend = null;
        List<int[]> foes = new List<int[]>();
        int[] ball = null;

        int team = me.GetComponent<MovementHandler>().myteam;

        for (int i = 0; i < gm.grid.GetLength(0); i++)
        {
            for (int j = 0; j < gm.grid.GetLength(1); j++)
            {
                foreach(var proj in gm.grid[i,j].projections)
                {
                    if (proj.t == 0)
                    {
                        if (proj.obj == me)
                        {
                            self = new int[] { i, j };
                        }
                        else if (proj.obj.GetComponent<BallHandler>() != null)
                        {
                            ball = new int[] { i, j };
                        }
                        else if (proj.obj.GetComponent<MovementHandler>() != null)
                        {
                            if (proj.obj.GetComponent<MovementHandler>().myteam == team)
                            {
                                friend = new int[] { i, j };
                            }
                            else
                            {
                                foes.Add(new int[] { i, j });
                            }
                        }
                    }
                }
            }
        }

        if(self != null)
        {
            gm.display.searchPaths.Clear();
            pathFriend.Clear();
            pathFoe1.Clear();
            pathFoe2.Clear();
            pathBall.Clear();

            if (friend != null)
            {
                var fp = gm.BuildPath(self[0], self[1], friend[0], friend[1]);
                if(fp.Count >= 2)
                {
                    for (int i = 0; i < fp.Count; i++)
                    {
                        pathFriend.Add(new Vector3(gm.startPos.x + fp[i][0] * gm.incrX + gm.incrX / 2, gm.startPos.y + fp[i][1] * gm.incrY + gm.incrY / 2, 0));
                    }
                    gm.display.searchPaths.Add(pathFriend);
                    gm.display.searchColors.Add(Color.green);

                }
            }
            else
            {
                Debug.LogError("JPS Cannot Find Friend :(");
            }

            if(foes.Count == 2)
            {
                var fp = gm.BuildPath(self[0], self[1], foes[0][0], foes[0][1]);
                if (fp.Count >= 2)
                {
                    for (int i = 0; i < fp.Count; i++)
                    {
                        pathFoe1.Add(new Vector3(gm.startPos.x + fp[i][0] * gm.incrX + gm.incrX / 2, gm.startPos.y + fp[i][1] * gm.incrY + gm.incrY / 2, 0));
                    }
                    gm.display.searchPaths.Add(pathFoe1);
                    gm.display.searchColors.Add(Color.red);

                }
                fp = gm.BuildPath(self[0], self[1], foes[1][0], foes[1][1]);
                if (fp.Count >= 2)
                {
                    for (int i = 0; i < fp.Count; i++)
                    {
                        pathFoe2.Add(new Vector3(gm.startPos.x + fp[i][0] * gm.incrX + gm.incrX / 2, gm.startPos.y + fp[i][1] * gm.incrY + gm.incrY / 2, 0));
                    }
                    gm.display.searchPaths.Add(pathFoe2);
                    gm.display.searchColors.Add(Color.red);

                }
            }
            else
            {
                Debug.LogError("JPS Cannot Find 2 Foes :(");
            }

            if (ball != null)
            {
                var fp = gm.BuildPath(self[0], self[1], ball[0], ball[1]);
                if (fp.Count >= 2)
                {
                    for (int i = 0; i < fp.Count; i++)
                    {
                        pathBall.Add(new Vector3(gm.startPos.x + fp[i][0] * gm.incrX + gm.incrX / 2, gm.startPos.y + fp[i][1] * gm.incrY + gm.incrY / 2, 0));
                    }
                    gm.display.searchPaths.Add(pathBall);
                    gm.display.searchColors.Add(Color.yellow);
                }
            }

            bool hasBall = ballRef.isGrabbed && ballRef.LastShooter == me.GetComponent<AIMovementHandler>().Index;
            bool friendBall = ballRef.isGrabbed && ballRef.LastShooter == me.GetComponent<AIMovementHandler>().FriendTransform.gameObject.GetComponent<MovementHandler>().Index;
            bool enemyBall = ballRef.isGrabbed && ballRef.lastShooterTeam != me.GetComponent<AIMovementHandler>().myteam;

            bool seeFriend = false;
            if(pathFriend.Count > 0) seeFriend = !Physics.Raycast(me.transform.position, pathFriend[1], (pathFriend[1] - me.transform.position).magnitude);

            bool seeEnemy1 = false;
            if(pathFoe1.Count > 0) seeEnemy1 = !Physics.Raycast(me.transform.position, pathFoe1[1], (pathFoe1[1] - me.transform.position).magnitude);

            bool seeEnemy2 = false;
            if(pathFoe2.Count > 2) seeEnemy2 = !Physics.Raycast(me.transform.position, pathFoe2[1], (pathFoe2[1] - me.transform.position).magnitude);

            //if (me.gameObject.GetComponent<AIMovementHandler>().Index == XInputDotNetPure.PlayerIndex.One) Debug.Log("Yo " + seeFriend + " " + seeEnemy1 + " " + seeEnemy2);

            bool canAttack = 
                (gm.view.gamemanager.Team1.BarLevel == 1 && me.GetComponent<AIMovementHandler>().myteam == 1) || 
                (gm.view.gamemanager.Team2.BarLevel == 1 && me.GetComponent<AIMovementHandler>().myteam == 2);
            bool enemyCanAttack =
                (gm.view.gamemanager.Team1.BarLevel == 1 && me.GetComponent<AIMovementHandler>().myteam == 2) ||
                (gm.view.gamemanager.Team2.BarLevel == 1 && me.GetComponent<AIMovementHandler>().myteam == 1);

            Vector3 targetVector = new Vector3();

            if (!passOn) passOn = true;
            if (!shootOn) shootOn = true;

            if (hasBall)
            {
                if(canAttack)
                {
                    if(seeEnemy1)
                    {
                        //shoot
                        targetVector = (pathFoe1[1] - me.transform.position).normalized;
                        shootOn = false;
                    }
                    else if(seeEnemy2)
                    {
                        targetVector = (pathFoe2[1] - me.transform.position).normalized;
                        shootOn = false;
                    }
                    else
                    {
                        if (Time.time % 60 > 30) targetVector = (pathFoe1[1] - me.transform.position).normalized;
                        else targetVector = (pathFoe2[1] - me.transform.position).normalized;
                    }
                }
                else
                {
                    if(seeFriend)
                    {
                        //move to ally
                        targetVector = (pathFriend[1] - me.transform.position).normalized;
                        passOn = true;
                    }
                    else
                    {
                        //flee
                        targetVector =
                            ((pathFoe1[1] - me.transform.position).normalized / (pathFoe1[1] - me.transform.position).magnitude +
                            (pathFoe2[1] - me.transform.position).normalized / (pathFoe2[1] - me.transform.position).magnitude).normalized;
                    }
                }
            }
            else if(friendBall)
            {
                //move towards and maintain distance friend
                if (pathFriend.Count > 0 && (pathFriend[1] - me.transform.position).magnitude > 5)
                {
                    targetVector = (pathFriend[1] - me.transform.position).normalized;
                }
                else
                {
                    targetVector = ((me.transform.position - lastPosition).normalized + new Vector3(Random.value / 5f, Random.value / 5f, 0)).normalized;
                }
            }
            else if(enemyBall)
            {
                if(enemyCanAttack)
                {
                    //flee
                    targetVector =
                        ((pathFoe1[1] - me.transform.position).normalized / (pathFoe1[1] - me.transform.position).magnitude +
                        (pathFoe2[1] - me.transform.position).normalized / (pathFoe2[1] - me.transform.position).magnitude).normalized;
                }
                else
                {
                    //intercept
                    targetVector = ((pathFoe1[1] + pathFoe2[1]) / 2 - me.transform.position).normalized;
                }
            }
            else
            {
                //move to ball
                if(pathBall.Count > 0) targetVector = (pathBall[1] - me.transform.position).normalized;
            }

            horizontalSpeed = Mathf.Clamp(targetVector.x, -1f, 1f);

            targetingX = targetVector.x;
            targetingY = targetVector.y;

            if (targetVector.y > 0.5f && (me.transform.position - lastPosition).y < 0.1f) jumpOn = true;

            if (pathFoe1.Count > 0 && (pathFoe1[1] - me.transform.position).magnitude < 2 && Vector3.Dot(targetVector, (pathFoe1[1] - me.transform.position).normalized) > 0.8f) dashOn = true;

            if (pathFoe2.Count > 0 && (pathFoe2[1] - me.transform.position).magnitude < 2 && Vector3.Dot(targetVector, (pathFoe2[1] - me.transform.position).normalized) > 0.8f) dashOn = true;

        }
        else
        {
            Debug.LogError("JPS Cannot Find Self :(");
        }

        lastPosition = me.transform.position;
    }
}