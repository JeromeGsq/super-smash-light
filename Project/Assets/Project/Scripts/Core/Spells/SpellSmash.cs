using GamepadInput;
using UnityEngine;

public class SpellSmash : MonoBehaviour
{
	private bool canSmash;

	private GameObject spawnedSight;
	private Vector3 t;

	public GameObject Sight;

    private GamepadState gamepadState;

    private void Awake()
	{
		this.t = Vector3.one;
        gamepadState = GetComponent<Player>().gamepadState;
	}

	private void Update()
	{
		if(this.canSmash == true)
		{
			if(gamepadState.RightStickAxis.x != 0)
			{
				this.t.x = gamepadState.RightStickAxis.x;
			}
			if(gamepadState.RightStickAxis.y != 0)
			{
				this.t.y = gamepadState.RightStickAxis.y;
			}

			this.t.z = 0f;
            print(gamepadState.RightStickAxis.x);
			this.spawnedSight.transform.position = this.transform.position + (this.t.normalized * 2);
		}
	}

	public void DoSpell()
	{
		this.canSmash = true;
		this.spawnedSight = Instantiate(this.Sight, this.transform.position, Quaternion.identity);
	}

	public void UnSpell()
	{
		GetComponent<Player>().RetBall().GetComponent<Ball>().Smash(this.spawnedSight.transform.position);
		canSmash = false;
		Destroy(this.spawnedSight);
	}
}
