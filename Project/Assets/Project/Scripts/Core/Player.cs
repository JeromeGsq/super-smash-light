using GamepadInput;
using UnityEngine;

public class Player : MonoBehaviour
{
	private bool widthLocked;
	private Vector2 contactUnlock;

	private new Rigidbody2D rigidbody;

	private int nbJump;
	private bool freeIn;
	private float freeInTimerMax;
	private float freeInTimer;
	private bool dash;
	private float dashTimer;
	public float globalDashTimer;
	private float localDashTimer;
	private bool dashing;

    public ip_GamePad.Index ControllerIndex;
    public GamepadState gamepadState;

    public int PlayerId;
	public GameObject PlayerFriend;
	public GameObject Ball;
	public string Team;

	public int JumpForce;
	public int Speed;

	public PlayerState State;
	public Direction Direction;

    private void Awake()
    {
        this.gamepadState = new GamepadState();
    }

    private void Start()
	{
		this.localDashTimer = 0f;
		this.dashTimer = 0f;
		this.nbJump = 0;
		this.freeInTimerMax = 0f;
		this.freeInTimer = 0f;
		this.freeIn = false;
		this.rigidbody = this.transform.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
        this.InputController();
        this.JoyController();
		if(this.freeIn == true)
		{
			if(this.freeInTimer >= this.freeInTimerMax)
			{
				this.SetState(PlayerState.Grounded);
				this.freeIn = false;
				this.freeInTimer = 0f;
			}
			this.freeInTimer += Time.deltaTime;
		}

		if(this.dash == true)
		{
			if(this.dashing == true)
			{
				if(this.dashTimer >= 0.08f)
				{
					Debug.Log("FIN DE DASH");
					this.dashing = false;
					this.dashTimer = 0f;
					this.rigidbody.gravityScale = 1f;
					this.rigidbody.velocity = Vector2.zero;
				}
				this.dashTimer += Time.deltaTime;
			}
			if(this.localDashTimer >= this.globalDashTimer)
			{
				this.dash = false;
				this.localDashTimer = 0f;
			}
			this.localDashTimer += Time.deltaTime;
		}
	}

	public bool HasBall()
	{
		if(this.transform.Find("Hand").transform.Find(this.Ball.name) != null)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public GameObject RetBall()
	{
		return (this.transform.Find("Hand").transform.Find(this.Ball.name).gameObject);
	}

	private void PassBall(Vector3 direction)
	{
		if(this.HasBall())
		{
			this.Ball.GetComponent<Ball>().Pass(direction);
		}
	}

	public void InputController()
	{
        ip_GamePad.GetState(ref this.gamepadState, ControllerIndex);

        if (this.gamepadState.APressed == true)
        {
            Debug.Log("La touche A est enfoncée");
        }
        if (this.gamepadState.APressed)
		{
			this.PassBall(PlayerFriend.transform.position);
		}
		else if(this.gamepadState.YPressed || this.gamepadState.XPressed)
		{
			this.Jump();
		}
		else if(this.gamepadState.RBPressed)
		{
			this.GetComponent<SpellSmash>().DoSpell();
		}
		else if(this.gamepadState.LBPressed)
		{
			this.GetComponent<SpellSmash>().UnSpell();
		}
	}

	public void Dash(float hor, float ver)
	{
		if(this.dash == true)
		{
			return;
		}

		Vector3 tmp;

		this.rigidbody.gravityScale = 0f;
		this.rigidbody.velocity = Vector2.zero;

		tmp.z = 0;
		tmp.x = hor;
		tmp.y = ver;
		this.dashing = true;
		tmp = tmp.normalized;
		this.rigidbody.AddForce(tmp * Speed * 5, ForceMode2D.Impulse);

		Debug.Log(tmp);
	}

	private void Jump()
	{
		if(this.nbJump >= 1)
		{
			return;
		}

		Vector2 v;

		v = this.rigidbody.velocity;
		v.y = 0;
		this.rigidbody.velocity = v;

		this.transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * this.JumpForce, ForceMode2D.Impulse);

		this.nbJump++;
	}

	public void JoyController()
	{
		if(gamepadState.LeftStickAxis.x != 0)
		{
			this.Move(gamepadState.LeftStickAxis.x);
		}

	}

	public void ResetMove()
	{
		this.Move(0);
	}

	private void Move(float pos)
	{
		if(this.State == PlayerState.Stunt || this.dashing == true)
		{
			return;
		}

		Vector2 t;

		t = Vector2.right * pos;
		if(this.contactUnlock.x == t.x || this.widthLocked == false)
		{
			//this.transform.GetComponent<Rigidbody2D>().AddForce(t * speed, ForceMode2D.Impulse);
			Vector2 v;
			v = this.rigidbody.velocity;
			v.x = t.x * Speed;
			this.rigidbody.velocity = v;
		}
	}

	public void SetTeam(string t)
	{
		this.Team = t;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		Vector2 contact = collision.contacts[0].normal;
		if(contact.x == 1 || contact.x == -1)
		{
			this.contactUnlock = contact;
			this.widthLocked = true;
		}
		if(contact.y == 1)
		{
			this.nbJump = 0;
		}
	}

	public void SetState(PlayerState s)
	{
		State = s;
		if(s == PlayerState.Stunt)
		{
			this.freeIn = true;
		}
		else
		{
			this.freeIn = false;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		this.widthLocked = false;
	}
}
