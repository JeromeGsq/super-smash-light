using UnityEngine;
using Prime31;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameMenuManager2 : MonoBehaviour {

    static public int gamepad1team;
    static public int gamepad2team;
    static public int gamepad3team;
    static public int gamepad4team;

    [SerializeField]
    static public int gamepad1color;
    [SerializeField]
    static public int gamepad2color;
    [SerializeField]
    static public int gamepad3color;
    [SerializeField]
    static public int gamepad4color;

    private bool gamepad1Validated;
    private bool gamepad2Validated;
    private bool gamepad3Validated;
    private bool gamepad4Validated;

    private Color Blue1 = new Color(0.133f, 0.517f, 0.949f);
    private Color Blue2 = new Color(0.082f, 0.211f, 0.745f);
    private Color Violet = new Color(0.317f, 0.133f, 0.831f);
    private Color Green = new Color(0.047f, 0.694f, 0.239f);
    private Color Purple = new Color(0.753f, 0.192f, 0.588f);
    private Color Yellow = new Color(0.949f, 0.701f, 0.113f);
    private Color Orange = new Color(0.976f, 0.325f, 0.270f);
    private Color Red = new Color(0.854f, 0.137f, 0.152f);

    private SpriteRenderer g1, g2, g3, g4, sg1, sg2, sg3, sg4;

    bool g1left, g2left, g3left, g4left;
    bool g1right, g2right, g3right, g4right;
    bool g1up, g2up, g3up, g4up;
    bool g1a, g2a, g3a, g4a;
    bool g1y;

    [Space(10)]
    [SerializeField]
    private GameObject gamepad1;
    [Space(10)]
    [SerializeField]
    private GameObject gamepad2;
    [Space(10)]
    [SerializeField]
    private GameObject gamepad3;
    [Space(10)]
    [SerializeField]
    private GameObject gamepad4;

    private GamePadState gamepadState1;
    private GamePadState gamepadState2;
    private GamePadState gamepadState3;
    private GamePadState gamepadState4;

    private GamePadState gamepadState;

    [Space(10)]
    [SerializeField]
    private Sprite greyGamepad;

    [Space(10)]
    [SerializeField]
    private Sprite whiteGamepad;

    [Space(10)]
    [SerializeField]
    private GameObject continueMessage;
    [Space(10)]
    [SerializeField]
    private GameObject alerteMessage;

    [Space(10)]
    [SerializeField]
    private GameObject AnimatedBandeau;

    [Space(10)]
    [SerializeField]
    private GameObject Menu3;

    [Space(10)]
    [SerializeField]
    private GameObject canvas;

    [Space(10)]
    [SerializeField]
    private GameObject menu1;

    [Space(10)]
    [SerializeField]
    private GameObject fade;

    private bool pressedBack;

    private bool rearrageAI;

    // Start is called before the first frame update
    void Start() {

    gamepad1Validated = false;
    gamepad2Validated = false;
    gamepad3Validated = false;
    gamepad4Validated = false;
 

        gamepad1team = 0;
        gamepad2team = 0;
        gamepad3team = 0;
        gamepad4team = 0;

        gamepad1color = 0;
        gamepad2color = 0;
        gamepad3color = 0;
        gamepad4color = 0;

        this.gamepadState1 = GamePad.GetState(PlayerIndex.One);
        this.gamepadState2 = GamePad.GetState(PlayerIndex.Two);
        this.gamepadState3 = GamePad.GetState(PlayerIndex.Three);
        this.gamepadState4 = GamePad.GetState(PlayerIndex.Four);
 
        

    }
    void OnEnable() {

        gamepad1Validated = false;
        gamepad2Validated = false;
        gamepad3Validated = false;
        gamepad4Validated = false;

        g1 = GameObject.Find("gamepad1").GetComponent<SpriteRenderer>();
        g1.sprite = greyGamepad;
        g2 = GameObject.Find("gamepad2").GetComponent<SpriteRenderer>();
        g2.sprite = greyGamepad;
        g3 = GameObject.Find("gamepad3").GetComponent<SpriteRenderer>();
        g3.sprite = greyGamepad;
        g4 = GameObject.Find("gamepad4").GetComponent<SpriteRenderer>();
        g4.sprite = greyGamepad;

        gamepad1team = 0;
        gamepad2team = 0;
        gamepad3team = 0;
        gamepad4team = 0;
        gamepad1color = 0;
        gamepad2color = 0;
        gamepad3color = 0;
        gamepad4color = 0;

        sg1 = GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>();
        sg2 = GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>();
        sg3 = GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>();
        sg4 = GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>();

        StartCoroutine(CoroutineUtils.DelaySeconds(() => {
            AnimatedBandeau.SetActive(false);
            canvas.SetActive(true);
        }, 0.7f));
    }

    // Update is called once per frame
    void Update()
    {
        pressedBack = true;

        bool setToRearrange = rearrageAI;
        if(rearrageAI)
        {
            if (!gamepadState2.IsConnected) gamepad2team = 0;
            if (!gamepadState3.IsConnected) gamepad3team = 0;
            if (!gamepadState4.IsConnected) gamepad4team = 0;
            rearrageAI = false;
        }

        gamepadState1 = GamePad.GetState(PlayerIndex.One);
        gamepadState2 = GamePad.GetState(PlayerIndex.Two);
        gamepadState3 = GamePad.GetState(PlayerIndex.Three);
        gamepadState4 = GamePad.GetState(PlayerIndex.Four);

        // To only activate once per button press

        if (gamepadState1.Buttons.A == ButtonState.Released) g1a = false;
        if (gamepadState2.Buttons.A == ButtonState.Released) g2a = false;
        if (gamepadState3.Buttons.A == ButtonState.Released) g3a = false;
        if (gamepadState4.Buttons.A == ButtonState.Released) g4a = false;

        if (gamepadState1.DPad.Left == ButtonState.Released) g1left = false;
        if (gamepadState2.DPad.Left == ButtonState.Released) g2left = false;
        if (gamepadState3.DPad.Left == ButtonState.Released) g3left = false;
        if (gamepadState4.DPad.Left == ButtonState.Released) g4left = false;

        if (gamepadState1.DPad.Right == ButtonState.Released) g1right = false;
        if (gamepadState2.DPad.Right == ButtonState.Released) g2right = false;
        if (gamepadState3.DPad.Right == ButtonState.Released) g3right = false;
        if (gamepadState4.DPad.Right == ButtonState.Released) g4right = false;

        if (gamepadState1.DPad.Up == ButtonState.Released) g1up = false;
        if (gamepadState2.DPad.Up == ButtonState.Released) g2up = false;
        if (gamepadState3.DPad.Up == ButtonState.Released) g3up = false;
        if (gamepadState4.DPad.Up == ButtonState.Released) g4up = false;

        if (gamepadState1.Buttons.Y == ButtonState.Released) g1y = false;

        // GamePad 1 Controls
        if (!gamepad1Validated)
        {
            if (gamepadState1.DPad.Right == ButtonState.Pressed && !g1right)
            {
                g1right = true;
                if (gamepad1team == 0)
                {
                    gamepad1team = 2;
                    gamepad1color = 21;
                    rearrageAI = true;
                }
                if (gamepad1team == 1)
                {
                    gamepad1team = 0;
                    gamepad1color = 0;
                    rearrageAI = true;
                }
            }

            if(gamepadState1.DPad.Left == ButtonState.Pressed && !g1left)
            {
                g1left = true;
                if (gamepad1team == 0)
                {
                    gamepad1team = 1;
                    gamepad1color = 1;
                    rearrageAI = true;
                }
                if (gamepad1team == 2)
                {
                    gamepad1team = 0;
                    gamepad1color = 0;
                    rearrageAI = true;
                }
            }

            if (gamepad1team != 0)
            {
                if(gamepadState1.DPad.Up == ButtonState.Pressed && !g1up)
                {
                    g1up = true;
                    gamepad1color++;
                    if (gamepad1color == 5) gamepad1color = 1;
                    if (gamepad1color == 25) gamepad1color = 21;
                }
            }

            SetSpriteColor(sg1, gamepad1color);
        }

        if (gamepadState1.Buttons.A == ButtonState.Pressed && !g1a)
        {
            g1a = true;
            if (!gamepad1Validated && gamepad1color != 0)
            {
                gamepad1Validated = true;
                g1.sprite = whiteGamepad;
            }
            else if(gamepad1Validated)
            {
                gamepad1Validated = false;
                g1.sprite = greyGamepad;
            }
        }

        // ---------------------- Validation IA with Y ----------------------------------------

        if (gamepadState1.Buttons.Y == ButtonState.Pressed && gamepad1Validated && !g1y)
        {
            g1y = true;
            if (!gamepad2Validated && !gamepad3Validated && !gamepad4Validated)
            {
                gamepad2Validated = true;
                g2.sprite = whiteGamepad;
                gamepad3Validated = true;
                g3.sprite = whiteGamepad;
                gamepad4Validated = true;
                g4.sprite = whiteGamepad;
            }
            else
            {
                gamepad2Validated = false;
                g2.sprite = greyGamepad;
                gamepad3Validated = false;
                g3.sprite = greyGamepad;
                gamepad4Validated = false;
                g4.sprite = greyGamepad;
            }
        }

        //GamePad 2 Controls

        if (!gamepad2Validated)
        {
            if (gamepadState2.IsConnected)
            {
                if (gamepadState2.DPad.Right == ButtonState.Pressed && !g2right)
                {
                    g2right = true;
                    if (gamepad2team == 0)
                    {
                        gamepad2team = 2;
                        gamepad2color = 22;
                        rearrageAI = true;
                    }
                    if (gamepad2team == 1)
                    {
                        gamepad2team = 0;
                        gamepad2color = 0;
                        rearrageAI = true;
                    }
                }

                if (gamepadState2.DPad.Left == ButtonState.Pressed && !g2left)
                {
                    g2left = true;
                    if (gamepad2team == 0)
                    {
                        gamepad2team = 1;
                        gamepad2color = 2;
                        rearrageAI = true;
                    }
                    if (gamepad2team == 2)
                    {
                        gamepad2team = 0;
                        gamepad2color = 0;
                        rearrageAI = true;
                    }
                }
            }
            else if (setToRearrange)
            {
                if (teamCounter(true) < 2)
                {
                    gamepad2team = 3;
                    gamepad2color = 2;
                }
                else
                {
                    gamepad2team = 4;
                    gamepad2color = 22;
                }
            }

            if (gamepad2team != 0)
            {
                if (gamepadState2.DPad.Up == ButtonState.Pressed && !g1up)
                {
                    g1up = true;
                    gamepad2color++;
                    if (gamepad2color == 5) gamepad2color = 1;
                    if (gamepad2color == 25) gamepad2color = 21;
                }
            }

            SetSpriteColor(sg2, gamepad2color);
        }

        if (gamepadState2.Buttons.A == ButtonState.Pressed && !g2a)
        {
            g2a = true;
            if (!gamepad2Validated && gamepad2color != 0)
            {
                gamepad2Validated = true;
                g2.sprite = whiteGamepad;
            }
            else if (gamepad2Validated)
            {
                gamepad2Validated = false;
                g2.sprite = greyGamepad;
            }
        }

        // GamePad 3 Controls
        if (!gamepad3Validated)
        {
            if (gamepadState3.IsConnected)
            {
                if (gamepadState3.DPad.Right == ButtonState.Pressed && !g3right)
                {
                    g3right = true;
                    if (gamepad3team == 0)
                    {
                        gamepad3team = 2;
                        gamepad3color = 23;
                        rearrageAI = true;
                    }
                    if (gamepad3team == 1)
                    {
                        gamepad3team = 0;
                        gamepad3color = 0;
                        rearrageAI = true;
                    }
                }

                if (gamepadState3.DPad.Left == ButtonState.Pressed && !g3left)
                {
                    g3left = true;
                    if (gamepad3team == 0)
                    {
                        gamepad3team = 1;
                        gamepad3color = 3;
                        rearrageAI = true;
                    }
                    if (gamepad3team == 2)
                    {
                        gamepad3team = 0;
                        gamepad3color = 0;
                        rearrageAI = true;
                    }
                }
            }
            else if (setToRearrange)
            {
                if (teamCounter(true) < 2)
                {
                    gamepad3team = 3;
                    gamepad3color = 3;
                }
                else
                {
                    gamepad3team = 4;
                    gamepad3color = 23;
                }
            }

            if (gamepad3team != 0)
            {
                if (gamepadState3.DPad.Up == ButtonState.Pressed && !g3up)
                {
                    g3up = true;
                    gamepad3color++;
                    if (gamepad3color == 5) gamepad3color = 1;
                    if (gamepad3color == 25) gamepad3color = 21;
                }
            }
            SetSpriteColor(sg3, gamepad3color);
        }

        if (gamepadState3.Buttons.A == ButtonState.Pressed && !g3a)
        {
            g3a = true;
            if (!gamepad3Validated && gamepad3color != 0)
            {
                gamepad3Validated = true;
                g3.sprite = whiteGamepad;
            }
            else if (gamepad3Validated)
            {
                gamepad3Validated = false;
                g3.sprite = greyGamepad;
            }
        }

        //GamePad 4 controls
        if (!gamepad4Validated)
        {
            if (gamepadState4.IsConnected)
            {
                if (gamepadState4.DPad.Right == ButtonState.Pressed && !g4right)
                {
                    g4right = true;
                    if (gamepad4team == 0)
                    {
                        gamepad4team = 2;
                        gamepad4color = 24;
                        rearrageAI = true;
                    }
                    if (gamepad4team == 1)
                    {
                        gamepad4team = 0;
                        gamepad4color = 0;
                        rearrageAI = true;
                    }
                }

                if (gamepadState4.DPad.Left == ButtonState.Pressed && !g4left)
                {
                    g4left = true;
                    if (gamepad4team == 0)
                    {
                        gamepad4team = 1;
                        gamepad4color = 4;
                        rearrageAI = true;
                    }
                    if (gamepad4team == 2)
                    {
                        gamepad4team = 0;
                        gamepad4color = 0;
                        rearrageAI = true;
                    }
                }
            }
            else if(setToRearrange)
            {
                if (teamCounter(true) < 2)
                {
                    gamepad4team = 3;
                    gamepad4color = 4;
                }
                else
                {
                    gamepad4team = 4;
                    gamepad4color = 24;
                }
            }

            if (gamepad4team != 0)
            {
                if (gamepadState4.DPad.Up == ButtonState.Pressed && !g4up)
                {
                    g4up = true;
                    gamepad4color++;
                    if (gamepad4color == 5) gamepad4color = 1;
                    if (gamepad4color == 25) gamepad4color = 21;
                }
            }
            SetSpriteColor(sg4, gamepad4color);
        }

        if (gamepadState4.Buttons.A == ButtonState.Pressed && !g4a)
        {
            g4a = true;
            if (!gamepad4Validated && gamepad4color != 0)
            {
                gamepad4Validated = true;
                g4.sprite = whiteGamepad;
            }
            else if (gamepad4Validated)
            {
                gamepad4Validated = false;
                g4.sprite = greyGamepad;
            }
        }

        // Validation

        if (gamepad1Validated && gamepad2Validated && gamepad3Validated && gamepad4Validated)
        {
            if (teamCounter(true) == 2 && teamCounter(false) == 2)
            {
                continueMessage.SetActive(true);
                if (gamepadState1.Buttons.Start == ButtonState.Pressed ||
                    gamepadState2.Buttons.Start == ButtonState.Pressed ||
                    gamepadState3.Buttons.Start == ButtonState.Pressed ||
                    gamepadState4.Buttons.Start == ButtonState.Pressed)
                {
                    this.gameObject.SetActive(false);
                    Menu3.SetActive(true);
                }
            }
            else
            {
                alerteMessage.SetActive(true);
            }

        }
        else
        {
            continueMessage.SetActive(false);
            alerteMessage.SetActive(false);
        }

        if (gamepadState1.Buttons.Back == ButtonState.Pressed ||
            gamepadState2.Buttons.Back == ButtonState.Pressed ||
            gamepadState3.Buttons.Back == ButtonState.Pressed ||
            gamepadState4.Buttons.Back == ButtonState.Pressed)
        {
            canvas.SetActive(false);
            fade.SetActive(false);
            fade.SetActive(true);
            pressedBack = false;
            StartCoroutine(CoroutineUtils.DelaySeconds(() =>
            {
                gamepad1team = 0;
                gamepad2team = 0;
                gamepad3team = 0;
                gamepad4team = 0;
                gamepad1color = 0;
                gamepad2color = 0;
                gamepad3color = 0;
                gamepad4color = 0;
                this.menu1.SetActive(true);
                this.gameObject.SetActive(false);
            }, 0.8f));
        }

        // for debug , appuyer sur espace pour passer directement au menu suivant avec les manettes pre-valider

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gamepad1team = 1;
            gamepad2team = 1;
            gamepad3team = 2;
            gamepad4team = 2;
            gamepad1color = 1;
            gamepad2color = 3;
            gamepad3color = 21;
            gamepad4color = 23;
            this.gameObject.SetActive(false);
            Menu3.SetActive(true);

        }

        SetControllerImagePositions();

        TData savedData = FindObjectOfType<TData>();

        savedData.p1 = gamepad1team;
        savedData.p2 = gamepad2team;
        savedData.p3 = gamepad3team;
        savedData.p4 = gamepad4team;

        DontDestroyOnLoad(savedData.gameObject);
    }

    private void SetControllerImagePositions()
    {

        switch (gamepad1team)
        {
            case 1:
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(-8.33f, 0f, -2.3f);
                break;
            case 2:
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(8.33f, 0f, -2.3f);
                break;
            case 3:
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(-8.33f, 0f, -2.3f);
                break;
            case 4:
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(8.33f, 0f, -2.3f);
                break;
            default:
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(0, 0, -2.3f);
                break;
        }
        switch (gamepad2team)
        {
            case 1:
                gamepad2.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -2.5f, -2.3f);
                break;
            case 2:
                gamepad2.GetComponent<Transform>().localPosition = new Vector3(8.33f, -2.5f, -2.3f);
                break;
            case 3:
                gamepad2.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -2.5f, -2.3f);
                break;
            case 4:
                gamepad2.GetComponent<Transform>().localPosition = new Vector3(8.33f, -2.5f, -2.3f);
                break;
            default:
                gamepad2.GetComponent<Transform>().localPosition = new Vector3(0, -2.5f, -2.3f);
                break;
        }
        switch (gamepad3team)
        {
            case 1:
                gamepad3.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -5f, -2.3f);
                break;
            case 2:
                gamepad3.GetComponent<Transform>().localPosition = new Vector3(8.33f, -5f, -2.3f);
                break;
            case 3:
                gamepad3.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -5f, -2.3f);
                break;
            case 4:
                gamepad3.GetComponent<Transform>().localPosition = new Vector3(8.33f, -5f, -2.3f);
                break;
            default:
                gamepad3.GetComponent<Transform>().localPosition = new Vector3(0, -5f, -2.3f);
                break;
        }
        switch (gamepad4team)
        {
            case 1:
                gamepad4.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -7.5f, -2.3f);
                break;
            case 2:
                gamepad4.GetComponent<Transform>().localPosition = new Vector3(8.33f, -7.5f, -2.3f);
                break;
            case 3:
                gamepad4.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -7.5f, -2.3f);
                break;
            case 4:
                gamepad4.GetComponent<Transform>().localPosition = new Vector3(8.33f, -7.5f, -2.3f);
                break;
            default:
                gamepad4.GetComponent<Transform>().localPosition = new Vector3(0, -7.5f, -2.3f);
                break;
        }
    }

    private void SetSpriteColor(SpriteRenderer s, int index)
    {
        switch (index)
        {
            case 0:
                s.color = Color.white;
                break;
            case 1:
                s.color = Red;
                break;
            case 2:
                s.color = Orange;
                break;
            case 3:
                s.color = Yellow;
                break;
            case 4:
                s.color = Purple;
                break;
            case 21:
                s.color = Blue1;
                break;
            case 22:
                s.color = Blue2;
                break;
            case 23:
                s.color = Green;
                break;
            case 24:
                s.color = Violet;
                break;
            default:
                s.color = Color.white;
                break;
        }
    }

    private int teamCounter(bool team1)
    {
        int res = 0;
        if(team1)
        {
            if (gamepad1team == 1 || gamepad1team == 3) res++;
            if (gamepad2team == 1 || gamepad2team == 3) res++;
            if (gamepad3team == 1 || gamepad3team == 3) res++;
            if (gamepad4team == 1 || gamepad4team == 3) res++;
        }
        else
        {
            if (gamepad1team == 2 || gamepad1team == 4) res++;
            if (gamepad2team == 2 || gamepad2team == 4) res++;
            if (gamepad3team == 2 || gamepad3team == 4) res++;
            if (gamepad4team == 2 || gamepad4team == 4) res++;
        }
        return res;
    }
}

