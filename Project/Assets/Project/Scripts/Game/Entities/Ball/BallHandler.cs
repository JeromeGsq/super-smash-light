using System;
using Root.DesignPatterns;
using UnityEngine;
using XInputDotNetPure;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BallHandler : SceneSingleton<BallHandler>
{
    private Rigidbody2D rigidbody;
    private Collider2D collider;

    public PlayerIndex index;

    public bool isGrabbed = false;
    private bool engagePass = false;
    private bool engageShoot = false;

    private Vector3 lastKnownShootPosition;
    private PlayerIndex lastShooter;
    public int lastShooterTeam = 2;

    [Space(20)]

    [SerializeField]
    private TrailRenderer trail;

    [Space(20)]

    [SerializeField]
    private Material blue;
    [SerializeField]
    private Material red;
    [SerializeField]
    private Material yellow;
    [SerializeField]
    private Material white;

    [Space(20)]
    [SerializeField]
    private int rebond;

    [Space(20)]

    [Tooltip("Augmenter cette valeur pour gagner plus de % à chaque passes (defaut : 1)")]
    [SerializeField]
    private int barLevelAddScale = 1;

    public int myteam;

    public PlayerIndex Index
    {
        get => this.index;
        set
        {
            this.index = value;
            this.UpdateGameManagerBallIndex();
            Debug.Log("BH Changed " + Index);
        }
    }

    public bool EngagePass
    {
        get => this.engagePass;
        set => this.engagePass = value;
    }

    public bool EngageShoot
    {
        get => this.engageShoot;
        set => this.engageShoot = value;
    }

    public PlayerIndex LastShooter => this.lastShooter;

    public bool IsGrabbed => this.isGrabbed;

    private void Awake()
    {
        this.rigidbody = this.GetComponent<Rigidbody2D>();
        this.collider = this.GetComponent<Collider2D>();

        this.lastKnownShootPosition = this.transform.position;
    }

    private void Update()
    {
        //Debug.Log("Ball : " + index);
        //this.myteam = Team.GetTeam(this.Index);
        this.rigidbody.bodyType = this.isGrabbed ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
    }

    public void SetGrabbed(Transform ballAnchor, PlayerIndex index, int team)
    {
        this.rebond = 0;
        this.collider.isTrigger = true;
        this.Index = index;
        this.transform.SetParent(ballAnchor);
        this.transform.localPosition = Vector3.zero;
        this.isGrabbed = true;
        this.myteam = team;
        this.trail.material = myteam == 1 ? this.blue : this.red;

        // if the last shooter is my teammate
        if ((myteam == lastShooterTeam)
            && this.engagePass == true)
        {
            GameManager.Get.AddBarLevel(
                Mathf.Abs(Vector3.Distance(this.lastKnownShootPosition, ballAnchor.position)) * ((float)this.barLevelAddScale / 100),
                myteam
            );

            this.engagePass = false;
        }

        this.lastShooter = index;
        this.lastShooterTeam = team;
    }

    public void Shoot(Transform target, float power, ShootType shootType)
    {
        if (target == null)
        {
            Debug.LogError("BallHandler : Shoot() : Unable to find target. Stop Shoot()");
            return;
        }

        this.lastKnownShootPosition = this.transform.position;

        this.transform.SetParent(null);
        Vector3 dir = (target.position - this.transform.position).normalized;

        switch (shootType)
        {
            case ShootType.Pass:
                this.trail.material = myteam == 1 ? this.blue : this.red;
                this.rigidbody.gravityScale = 0;
                // this.Index = Index.Any;
                this.engagePass = true;
                break;

            case ShootType.Shoot:
                this.trail.material = this.yellow;
                this.rigidbody.gravityScale = 0;
                GameManager.Get.ResetBarLevel(myteam);

                this.engageShoot = true;
                break;
        }

        this.collider.isTrigger = false;

        this.rigidbody.bodyType = RigidbodyType2D.Dynamic;
        this.rigidbody.AddForce(dir * power, ForceMode2D.Impulse);

        this.isGrabbed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.rebond += 1;

        if (rebond >=2 && collision.collider.CompareTag(Tags.Wall) && this.IsGrabbed == false)
        {
            this.rigidbody.gravityScale = 1;
            this.engagePass = false;
            this.engageShoot = false;
            this.Index = PlayerIndex.One;
            this.trail.material = this.white;
            this.rebond = 0;
        }
    }

    private void UpdateGameManagerBallIndex()
    {
        GameManager.Get.SetBallIndex(this.Index);
    }
}
