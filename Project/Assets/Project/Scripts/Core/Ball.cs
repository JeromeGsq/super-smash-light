using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
	private int score;
	private float weight;

	private Vector3 target;

	private ScoreManager scoreManager;

	private new Rigidbody2D rigidbody;
	private CircleCollider2D circlCollider;

	private BallState state;

	public int Speed;
	public int PowerBounce;
	public int MultScore;
	public string LastOwner;
	public GameObject TxtTeam;
    public GameObject ScoreMamangerGM;

	private void Awake()
	{
		this.rigidbody = this.transform.GetComponent<Rigidbody2D>();
		this.circlCollider = this.transform.GetComponent<CircleCollider2D>();
        this.scoreManager = this.ScoreMamangerGM.GetComponent<ScoreManager>();
		this.target = Vector3.zero;

		this.SetTeam("Unknow");
		this.SetState(BallState.Free);
	}

	private void Update()
	{
		if(this.state == BallState.OnPass)
		{
			this.InRun();
		}
	}

	public bool IsSmashing()
	{
		return this.state == BallState.OnSmash;
	}

	public void GetBall(GameObject player)
	{
		if(state == BallState.Locked)
		{
			return;
		}

		// It's good
		this.SetState(BallState.Locked);

		this.transform.parent = player.transform;
		this.transform.localRotation = Quaternion.identity;
		this.rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
		this.rigidbody.constraints = RigidbodyConstraints2D.None;

		// Nope
		if(player.transform.parent.gameObject.GetComponent<Player>().Team == LastOwner)
		{
			scoreManager.AddScore(score);
		}

		// Why ? 
		this.transform.position = player.transform.position;
		this.transform.position += Vector3.back;
		this.rigidbody.bodyType = RigidbodyType2D.Kinematic;
		this.SetTeam(player.transform.parent.gameObject.GetComponent<Player>().Team);
	}

	private void SetState(BallState state)
	{
		this.state = state;
		if(this.state != BallState.Locked)
		{
			this.rigidbody.bodyType = RigidbodyType2D.Dynamic;
		}
		if(this.state == BallState.OnPass || this.state == BallState.OnSmash)
		{
			this.rigidbody.gravityScale = 0f;
		}
		if(this.state == BallState.Locked || this.state == BallState.OnPass || this.state == BallState.OnSmash)
		{
			this.circlCollider.isTrigger = true;
			this.rigidbody.velocity = Vector2.zero;
			this.transform.parent = null;
		}
	}

	public void Free()
	{
		this.SetTeam("Unknow");
		this.transform.parent = null;
		this.circlCollider.isTrigger = false;
		this.rigidbody.gravityScale = 1f;
		if(state != BallState.OnPass && state != BallState.OnSmash)
		{
			this.rigidbody.velocity = Vector2.zero;
		}
		this.target = Vector3.zero;
		this.SetState(BallState.Free);
		this.scoreManager.LostCharge();
		this.score = 0;
	}

	public void Pass(Vector3 pos)
	{
		this.SetState(BallState.OnPass);

		this.target = (pos - this.transform.position).normalized * Time.deltaTime * this.Speed;
		this.rigidbody.AddForce(target * this.Speed, ForceMode2D.Impulse);
	}

	public void Smash(Vector3 pos)
	{
		this.SetState(BallState.OnSmash);

		this.target = (pos - this.transform.position).normalized * Time.deltaTime * this.Speed;
		this.rigidbody.AddForce(target * this.Speed, ForceMode2D.Impulse);
	}

	private void InRun()
	{
		this.score += (int)Mathf.Ceil(Time.deltaTime * this.MultScore);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(this.state == BallState.OnPass || this.state == BallState.OnSmash)
		{
			if(!other.name.Equals("Hand"))
			{
				this.Free();
			}

		}
	}

	private void SetTeam(string t)
	{
		this.LastOwner = t;
		this.TxtTeam.GetComponent<Text>().text = t;
	}
}
