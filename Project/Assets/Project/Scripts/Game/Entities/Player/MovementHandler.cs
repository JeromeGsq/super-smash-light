using UnityEngine;
using XInputDotNetPure;

public class MovementHandler : MonoBehaviour
{
    [Tooltip("Permettra de faire descendre plus radipedement ou de faire planner plus longtemps le joueur")]
    [SerializeField]
    protected float gravity = -60f;

    protected Transform pushSender;

    protected bool isPushed;

    protected float savedGravity;

    protected float pushPower;

    public Transform FriendTransform;

    private PlayerIndex index;

    public Vector3 MainPosition;

    public PlayerIndex Index
    {
        get
        {
            return index;
        }
        set
        {
            index = value;
            SetColors();
        }
    }

    [Tooltip("Le transform visuel du joueur qui sera flippé selon l'axe 'LocalScaleX'. Ce n'est que visuel")]
    public Transform player;

    public Team myteam;

    protected RuntimeAnimatorController color1;
    protected RuntimeAnimatorController color2;
    protected RuntimeAnimatorController color3;
    protected RuntimeAnimatorController color4;

    public RuntimeAnimatorController ColorBlue;
    public RuntimeAnimatorController ColorBlue2;
    public RuntimeAnimatorController ColorViolet;
    public RuntimeAnimatorController ColorGreen;
    public RuntimeAnimatorController ColorOrange;
    public RuntimeAnimatorController ColorRed;
    public RuntimeAnimatorController ColorYellow;
    public RuntimeAnimatorController ColorPurple;

    protected void SetColors()
    {
        var color = player.GetComponent<Animator>().runtimeAnimatorController;
        switch (Index)
        {
            case PlayerIndex.One:
                color = color1;
                break;
            case PlayerIndex.Two:
                color = color2;
                break;
            case PlayerIndex.Three:
                color = color3;
                break;
            case PlayerIndex.Four:
                color = color4;
                break;
        }

        player.GetComponent<Animator>().runtimeAnimatorController = color;
    }

    protected virtual void Awake()
    {

        ColorBlue = Resources.Load<RuntimeAnimatorController>("Animations/PlayerBlue");
        ColorBlue2 = Resources.Load<RuntimeAnimatorController>("Animations/PlayerBlue2");
        ColorViolet = Resources.Load<RuntimeAnimatorController>("Animations/PlayerViolet");
        ColorGreen = Resources.Load<RuntimeAnimatorController>("Animations/PlayerGreen");
        ColorOrange = Resources.Load<RuntimeAnimatorController>("Animations/PlayerOrange");
        ColorRed = Resources.Load<RuntimeAnimatorController>("Animations/PlayerRed");
        ColorYellow = Resources.Load<RuntimeAnimatorController>("Animations/PlayerYellow");
        ColorPurple = Resources.Load<RuntimeAnimatorController>("Animations/PlayerPurple");

        if (GameMenuManager2.gamepad1color == 21) { color1 = ColorRed; }
        if (GameMenuManager2.gamepad1color == 22) { color1 = ColorOrange; }
        if (GameMenuManager2.gamepad1color == 23) { color1 = ColorYellow; }
        if (GameMenuManager2.gamepad1color == 24) { color1 = ColorPurple; }
        if (GameMenuManager2.gamepad1color == 1) { color1 = ColorBlue; }
        if (GameMenuManager2.gamepad1color == 2) { color1 = ColorBlue2; }
        if (GameMenuManager2.gamepad1color == 3) { color1 = ColorGreen; }
        if (GameMenuManager2.gamepad1color == 4) { color1 = ColorViolet; }

        if (GameMenuManager2.gamepad2color == 21) { color2 = ColorRed; }
        if (GameMenuManager2.gamepad2color == 22) { color2 = ColorOrange; }
        if (GameMenuManager2.gamepad2color == 23) { color2 = ColorYellow; }
        if (GameMenuManager2.gamepad2color == 24) { color2 = ColorPurple; }
        if (GameMenuManager2.gamepad2color == 1) { color2 = ColorBlue; }
        if (GameMenuManager2.gamepad2color == 2) { color2 = ColorBlue2; }
        if (GameMenuManager2.gamepad2color == 3) { color2 = ColorGreen; }
        if (GameMenuManager2.gamepad2color == 4) { color2 = ColorViolet; }

        if (GameMenuManager2.gamepad3color == 21) { color3 = ColorRed; }
        if (GameMenuManager2.gamepad3color == 22) { color3 = ColorOrange; }
        if (GameMenuManager2.gamepad3color == 23) { color3 = ColorYellow; }
        if (GameMenuManager2.gamepad3color == 24) { color3 = ColorPurple; }
        if (GameMenuManager2.gamepad3color == 1) { color3 = ColorBlue; }
        if (GameMenuManager2.gamepad3color == 2) { color3 = ColorBlue2; }
        if (GameMenuManager2.gamepad3color == 3) { color3 = ColorGreen; }
        if (GameMenuManager2.gamepad3color == 4) { color3 = ColorViolet; }

        if (GameMenuManager2.gamepad4color == 21) { color4 = ColorRed; }
        if (GameMenuManager2.gamepad4color == 22) { color4 = ColorOrange; }
        if (GameMenuManager2.gamepad4color == 23) { color4 = ColorYellow; }
        if (GameMenuManager2.gamepad4color == 24) { color4 = ColorPurple; }
        if (GameMenuManager2.gamepad4color == 1) { color4 = ColorBlue; }
        if (GameMenuManager2.gamepad4color == 2) { color4 = ColorBlue2; }
        if (GameMenuManager2.gamepad4color == 3) { color4 = ColorGreen; }
        if (GameMenuManager2.gamepad4color == 4) { color4 = ColorViolet; }
    }

    // Messages
    public void Collision(Transform sender, float power)
    {
        if (isPushed == false)
        {
            //Debug.Log($"PlayerMovementHandler : Push() : pushed by {sender.name}");

            pushSender = sender;
            pushPower = power;
            isPushed = true;
            gravity = savedGravity;
        }
    }
}
