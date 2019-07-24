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

    private bool dpad1Up; 
    private bool dpad2Up; 
    private bool dpad3Up; 
    private bool dpad4Up;

    private bool dpad1R;
    private bool dpad2R;
    private bool dpad3R;
    private bool dpad4R;

    private bool dpad1L;
    private bool dpad2L;
    private bool dpad3L;
    private bool dpad4L;

    private bool btn1A;
    private bool btn2A;
    private bool btn3A;
    private bool btn4A;

    private bool btn1Y;

    private bool pressedBack;

    // Start is called before the first frame update
    void Start() {

        btn1Y = false;

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

        GameObject.Find("gamepad1").GetComponent<SpriteRenderer>().sprite = greyGamepad;
        GameObject.Find("gamepad2").GetComponent<SpriteRenderer>().sprite = greyGamepad;
        GameObject.Find("gamepad3").GetComponent<SpriteRenderer>().sprite = greyGamepad;
        GameObject.Find("gamepad4").GetComponent<SpriteRenderer>().sprite = greyGamepad;

        gamepad1team = 0;
        gamepad2team = 0;
        gamepad3team = 0;
        gamepad4team = 0;
        gamepad1color = 0;
        gamepad2color = 0;
        gamepad3color = 0;
        gamepad4color = 0;

        StartCoroutine(CoroutineUtils.DelaySeconds(() => {
            AnimatedBandeau.SetActive(false);
            canvas.SetActive(true);
        }, 0.7f));
    }
    // Update is called once per frame
    void Update() {

        pressedBack = true;


        this.gamepadState1 = GamePad.GetState(PlayerIndex.One);
        this.gamepadState2 = GamePad.GetState(PlayerIndex.Two);
        this.gamepadState3 = GamePad.GetState(PlayerIndex.Three);
        this.gamepadState4 = GamePad.GetState(PlayerIndex.Four);


        ///////////////////////////////////////////////////////////////////////////////
        
        //old pad script

        if(this.gamepadState1.DPad.Up == ButtonState.Released) {
            dpad1Up = false;
        }

        if(this.gamepadState2.DPad.Up == ButtonState.Released) {
            dpad2Up = false;
        }

        if(this.gamepadState3.DPad.Up == ButtonState.Released) {
            dpad3Up = false;
        }

        if(this.gamepadState4.DPad.Up == ButtonState.Released) {
            dpad4Up = false;
        }



        if(this.gamepadState1.DPad.Right == ButtonState.Released) {
            dpad1R = false;
        }

        if(this.gamepadState2.DPad.Right == ButtonState.Released) {
            dpad2R = false;
        }

        if(this.gamepadState3.DPad.Right == ButtonState.Released) {
            dpad3R = false;
        }

        if(this.gamepadState4.DPad.Right == ButtonState.Released) {
            dpad4R = false;
        }



        if(this.gamepadState1.DPad.Left == ButtonState.Released) {
            dpad1L = false;
        }

        if(this.gamepadState2.DPad.Left == ButtonState.Released) {
            dpad2L = false;
        }

        if(this.gamepadState3.DPad.Left == ButtonState.Released) {
            dpad3L = false;
        }

        if(this.gamepadState4.DPad.Left == ButtonState.Released) {
            dpad4L = false;
        }



        if(this.gamepadState1.Buttons.A == ButtonState.Released) {
            btn1A = false;
        }

        if(this.gamepadState2.Buttons.A == ButtonState.Released) {
            btn2A = false;
        }

        if(this.gamepadState3.Buttons.A == ButtonState.Released) {
            btn3A = false;
        }

        if(this.gamepadState4.Buttons.A == ButtonState.Released) {
            btn4A = false;
        }


        ///////////////////////////////////////////////////////////////////////////////




        if(gamepad1team == 0) {
            gamepad1.GetComponent<Transform>().localPosition = new Vector3(0, 0, -2.3f);
        }
        if(gamepad2team == 0) {
            gamepad2.GetComponent<Transform>().localPosition = new Vector3(0, -2.5f, -2.3f);
        }
        if(gamepad3team == 0) {
            gamepad3.GetComponent<Transform>().localPosition = new Vector3(0, -5f, -2.3f);
        }
        if(gamepad4team == 0) {
            gamepad4.GetComponent<Transform>().localPosition = new Vector3(0, -7.5f, -2.3f);
        }
        if(gamepad3color == 0) {
            GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        if(gamepad4color == 0) {
            GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        if(gamepad2color == 0) {
            GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        if(gamepad1color == 0) {
            GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        if(this.gamepad1Validated == false) {

            //if (this.gamepadState1.DPad.Right == ButtonState.Pressed && dpad1R == false)
            //{
            //    gamepad1team = 2;
            //    if (gamepad1color >= 20)
            //    {
            //        gamepad1color -= 20;
            //    }
            //    gamepad1.GetComponent<Transform>().localPosition = new Vector3(8.33f, 0, -2.3f);
            //    dpad1R = true;
            //}
            //if (this.gamepadState1.DPad.Left == ButtonState.Pressed && dpad1L == false)
            //{
            //    gamepad1team = 1;
            //    if (gamepad1color <= 21)
            //    {
            //        gamepad1color += 20;
            //    }
            //    gamepad1.GetComponent<Transform>().localPosition = new Vector3(-8.33f, 0, -2.3f);
            //    dpad1L = true;
            //}

            if (gamepad1team == 0 && this.gamepadState1.DPad.Right == ButtonState.Pressed && dpad1R == false)
            {
                gamepad1team = 2;
                if (gamepad1color >= 20)
                {
                    gamepad1color -= 20;
                }
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(8.33f, 0f, -2.3f);
                dpad1R = true;
            }

            if (gamepad1team == 2 && this.gamepadState3.DPad.Right == ButtonState.Pressed && dpad3R == false)
            {
                gamepad1team = 2;
                if (gamepad1color >= 20)
                {
                    gamepad1color -= 20;
                }
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(8.33f, 0f, -2.3f);
                dpad1R = true;
            }

            if (gamepad1team == 2 && this.gamepadState1.DPad.Left == ButtonState.Pressed && dpad1L == false)
            {
                gamepad1team = 0;
                if (gamepad1color <= 21)
                {
                    gamepad1color += 20;
                }
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, 0f);
                dpad1L = true;
            }

            if (gamepad1team == 0 && this.gamepadState1.DPad.Left == ButtonState.Pressed && dpad1L == false)
            {
                gamepad1team = 1;
                if (gamepad1color <= 21)
                {
                    gamepad1color += 20;
                }
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(-8.33f, 0f, -2.3f);
                dpad1L = true;
            }

            if (gamepad1team == 1 && this.gamepadState1.DPad.Right == ButtonState.Pressed && dpad1R == false)
            {
                gamepad1team = 0;
                if (gamepad1color >= 20)
                {
                    gamepad1color -= 20;
                }
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, 0f);
                dpad1R = true;
            }

        }
        if(gamepad1team != 0 && this.gamepad1Validated == false) {

            if(this.gamepadState1.DPad.Up == ButtonState.Pressed && dpad1Up == false) {
                gamepad1color += 1;
                dpad1Up = true;
            }
            if(gamepad1team == 2) {

                if(this.gamepadState1.Buttons.A == ButtonState.Pressed && btn1A == false && gamepad1color != 0) {
                    gamepad1Validated = true;
                    GameObject.Find("gamepad1").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                    btn1A = true;
                }

                if(gamepad1color > 4 && gamepad1color <= 5) {
                    gamepad1color = 1;
                }
            }
            if(gamepad1team == 1) {

                if(this.gamepadState1.Buttons.A == ButtonState.Pressed && btn1A == false && gamepad1color != 20) {
                    gamepad1Validated = true;
                    GameObject.Find("gamepad1").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                    btn1A = true;
                }

                if(gamepad1color > 24 & gamepad1color <= 25) {
                    gamepad1color = 21;
                }
            }
            if(gamepad3team == gamepad1team) {
                if(gamepad1color == gamepad3color) {
                    gamepad1color += 1;
                }
            }
            if(gamepad4team == gamepad1team) {
                if(gamepad1color == gamepad4color) {
                    gamepad1color += 1;
                }
            }
            if(gamepad2team == gamepad1team) {
                if(gamepad1color == gamepad2color) {
                    gamepad1color += 1;
                }
            }

            if(gamepad1color == 1) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = Red;
            }
            if(gamepad1color == 2) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = Orange;
            }
            if(gamepad1color == 3) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = Yellow;
            }
            if(gamepad1color == 4) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = Purple;
            }
            if(gamepad1color == 21) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = Blue1;
            }
            if(gamepad1color == 22) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = Blue2;
            }
            if(gamepad1color == 23) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = Green;
            }
            if(gamepad1color == 24) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = Violet;
            }
        }
        if(this.gamepad1Validated == true) {
            if(this.gamepadState1.Buttons.A == ButtonState.Pressed && btn1A == false) {
                GameObject.Find("gamepad1").GetComponent<SpriteRenderer>().sprite = greyGamepad;
                this.gamepad1Validated = false;
                btn1A = true;
            }
        }


        if (this.gamepad2Validated == false)
        {

            if (this.gamepadState2.DPad.Right == ButtonState.Pressed && dpad2R == false)
            {
                gamepad2team = 2;
                if (gamepad2color >= 20)
                {
                    gamepad2color -= 20;
                }
                gamepad2.GetComponent<Transform>().localPosition = new Vector3(8.33f, -2.5f, -2.3f);
                dpad2R = true;
            }
            if (this.gamepadState2.DPad.Left == ButtonState.Pressed && dpad2L == false)
            {
                gamepad2team = 1;
                if (gamepad2color <= 21)
                {
                    gamepad2color += 20;
                }
                gamepad2.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -2.5f, -2.3f);
                dpad2L = true;
            }
        }


        if (gamepad2team != 0 && this.gamepad2Validated == false) {


            if(this.gamepadState2.DPad.Up == ButtonState.Pressed && dpad2Up == false) {
                gamepad2color += 1;
                dpad2Up = true;
            }
            if(gamepad2team == 2) {

                if(this.gamepadState2.Buttons.A == ButtonState.Pressed && btn2A == false && gamepad2color != 0) {
                    gamepad2Validated = true;
                    GameObject.Find("gamepad2").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                    btn2A = true;
                }

                if(gamepad2color > 4 && gamepad2color <= 5) {
                    gamepad2color = 1;
                }
            }

            if(gamepad2team == 1) {

                if(this.gamepadState2.Buttons.A == ButtonState.Pressed && btn2A == false && gamepad2color != 20) {
                    gamepad2Validated = true;
                    GameObject.Find("gamepad2").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                    btn2A = true;
                }

                if(gamepad2color > 24 & gamepad2color <= 25) {
                    gamepad2color = 21;
                }
            }


                if (gamepad2team == 0)
            {

                if (this.gamepadState1.Buttons.Y == ButtonState.Pressed && btn1Y == false && gamepad1color != 20)
                {
                    gamepad2Validated = true;
                    GameObject.Find("gamepad2").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                    btn2A = true;
                }

                if (gamepad1color > 24 & gamepad1color <= 25)
                {
                    gamepad1color = 21;
                }
            }

            if (gamepad4team == gamepad2team) {
                if(gamepad2color == gamepad4color) {
                    gamepad2color += 1;
                }
            }
            if(gamepad3team == gamepad2team) {
                if(gamepad2color == gamepad3color) {
                    gamepad2color += 1;
                }
            }
            if(gamepad1team == gamepad2team) {
                if(gamepad2color == gamepad1color) {
                    gamepad2color += 1;
                }
            }

            if(gamepad2color == 1) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = Red;
            }
            if(gamepad2color == 2) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = Orange;
            }
            if(gamepad2color == 3) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = Yellow;
            }
            if(gamepad2color == 4) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = Purple;
            }
            if(gamepad2color == 21) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = Blue1;
            }
            if(gamepad2color == 22) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = Blue2;
            }
            if(gamepad2color == 23) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = Green;
            }
            if(gamepad2color == 24) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = Violet;
            }
        }
        if(this.gamepad2Validated == true) {
            if(this.gamepadState2.Buttons.A == ButtonState.Pressed && btn2A == false) {
                GameObject.Find("gamepad2").GetComponent<SpriteRenderer>().sprite = greyGamepad;
                this.gamepad2Validated = false;
                btn2A = true;
            }
        }


        if(this.gamepad3Validated == false) {

            if(this.gamepadState3.DPad.Right == ButtonState.Pressed && dpad3R == false) {
                gamepad3team = 2;
                if(gamepad3color >= 20) {
                    gamepad3color -= 20;
                }
                gamepad3.GetComponent<Transform>().localPosition = new Vector3(8.33f, -5f, -2.3f);
                dpad3R = true;
            }
            if(this.gamepadState3.DPad.Left == ButtonState.Pressed && dpad3L == false) {
                gamepad3team = 1;
                if(gamepad3color <= 21) {
                    gamepad3color += 20;
                }
                gamepad3.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -5f, -2.3f);
                dpad3L = true;
            }
        }
        if(gamepad3team != 0 && this.gamepad3Validated == false) {

            if(this.gamepadState3.DPad.Up == ButtonState.Pressed && dpad3Up == false) {
                gamepad3color += 1;
                dpad3Up = true;
            }
            if(gamepad3team == 2) {

                if(this.gamepadState3.Buttons.A == ButtonState.Pressed && btn3A == false && gamepad3color != 0) {
                    gamepad3Validated = true;
                    GameObject.Find("gamepad3").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                    btn3A = true;
                }

                if(gamepad3color > 4 && gamepad3color <= 5) {
                    gamepad3color = 1;
                }

            }
            if(gamepad3team == 1) {

                if(this.gamepadState3.Buttons.A == ButtonState.Pressed && btn3A == false && gamepad3color != 20) {
                    gamepad3Validated = true;
                    GameObject.Find("gamepad3").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                    btn3A = true;
                }

                if(gamepad3color > 24 & gamepad3color <= 25) {
                    gamepad3color = 21;
                }
            }

            if(gamepad4team == gamepad3team) {
                if(gamepad3color == gamepad4color) {
                    gamepad3color += 1;
                }
            }
            if(gamepad2team == gamepad3team) {
                if(gamepad3color == gamepad2color) {
                    gamepad3color += 1;
                }
            }
            if(gamepad1team == gamepad3team) {
                if(gamepad3color == gamepad1color) {
                    gamepad3color += 1;
                }
            }

            if(gamepad3color == 1) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = Red;
            }
            if(gamepad3color == 2) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = Orange;
            }
            if(gamepad3color == 3) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = Yellow;
            }
            if(gamepad3color == 4) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = Purple;
            }
            if(gamepad3color == 21) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = Blue1;
            }
            if(gamepad3color == 22) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = Blue2;
            }
            if(gamepad3color == 23) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = Green;
            }
            if(gamepad3color == 24) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = Violet;
            }

        }
        if(this.gamepad3Validated == true) {
            if(this.gamepadState3.Buttons.A == ButtonState.Pressed && btn3A == false) {
                GameObject.Find("gamepad3").GetComponent<SpriteRenderer>().sprite = greyGamepad;
                this.gamepad3Validated = false;
                btn3A = true;
            }
        }


        if(this.gamepad4Validated == false) {

            if(this.gamepadState4.DPad.Right == ButtonState.Pressed && dpad4R == false) {
                gamepad4team = 2;
                if(gamepad4color >= 20) {
                    gamepad4color -= 20;
                }
                gamepad4.GetComponent<Transform>().localPosition = new Vector3(8.33f, -7.5f, -2.3f);
                dpad4R = true;
            }
            if(this.gamepadState4.DPad.Left == ButtonState.Pressed && dpad4L == false) {
                gamepad4team = 1;
                if(gamepad4color <= 21) {
                    gamepad4color += 20;
                }
                gamepad4.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -7.5f, -2.3f);
                dpad4L = true;
            }
        }
        if(gamepad4team != 0 && this.gamepad4Validated == false) {

            if(this.gamepadState4.DPad.Up == ButtonState.Pressed && dpad4Up == false) {
                gamepad4color += 1;
                dpad4Up = true;
            }

            if(gamepad4team == 2) {

                if(this.gamepadState4.Buttons.A == ButtonState.Pressed && btn4A == false && gamepad4color != 0) {
                    gamepad4Validated = true;
                    GameObject.Find("gamepad4").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                    btn4A = true;
                }

                if(gamepad4color > 4 && gamepad4color <= 5) {
                    gamepad4color = 1;
                }
            }
            if(gamepad4team == 1) {

                if(this.gamepadState4.Buttons.A == ButtonState.Pressed && btn4A == false && gamepad4color != 20) {
                    gamepad4Validated = true;
                    GameObject.Find("gamepad4").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                    btn4A = true;
                }

                if(gamepad4color > 24 & gamepad4color <= 25) {
                    gamepad4color = 21;
                }
            }
            if(gamepad3team == gamepad4team) {
                if(gamepad4color == gamepad3color) {
                    gamepad4color += 1;
                }
            }
            if(gamepad2team == gamepad4team) {
                if(gamepad4color == gamepad2color) {
                    gamepad4color += 1;
                }
            }
            if(gamepad1team == gamepad4team) {
                if(gamepad4color == gamepad1color) {
                    gamepad4color += 1;
                }
            }

            if(gamepad4color == 1) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = Red;
            }
            if(gamepad4color == 2) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = Orange;
            }
            if(gamepad4color == 3) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = Yellow;
            }
            if(gamepad4color == 4) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = Purple;
            }
            if(gamepad4color == 21) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = Blue1;
            }
            if(gamepad4color == 22) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = Blue2;
            }
            if(gamepad4color == 23) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = Green;
            }
            if(gamepad4color == 24) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = Violet;
            }
        }
        if(this.gamepad4Validated == true) {
            if(this.gamepadState4.Buttons.A == ButtonState.Pressed && btn4A == false) {
                GameObject.Find("gamepad4").GetComponent<SpriteRenderer>().sprite = greyGamepad;
                this.gamepad4Validated = false;
                btn4A = true;
            }
        }
        for(int i = 0; i < 4; ++i) {
            PlayerIndex index = (PlayerIndex)i;
            this.gamepadState = GamePad.GetState(index);

            if(gamepad1Validated && gamepad2Validated && gamepad3Validated && gamepad4Validated) {
                // if all is well
                if(gamepad1team == 1)
                {
                    if (
                        (gamepad2team == 0 && gamepad3team == 0 && gamepad4team == 0) ||
                        (gamepad2team == 1 && gamepad3team == 0 && gamepad4team == 0) ||
                        (gamepad2team == 0 && gamepad3team == 1 && gamepad4team == 0) ||
                        (gamepad2team == 0 && gamepad3team == 0 && gamepad4team == 1) ||
                        (gamepad2team == 2 && gamepad3team == 0 && gamepad4team == 0) ||
                        (gamepad2team == 0 && gamepad3team == 2 && gamepad4team == 0) ||
                        (gamepad2team == 0 && gamepad3team == 0 && gamepad4team == 2) ||
                        (gamepad2team == 2 && gamepad3team == 2 && gamepad4team == 0) ||
                        (gamepad2team == 2 && gamepad3team == 0 && gamepad4team == 2) ||
                        (gamepad2team == 0 && gamepad3team == 2 && gamepad4team == 2) ||
                        (gamepad2team == 1 && gamepad3team == 0 && gamepad4team == 2) ||
                        (gamepad2team == 1 && gamepad3team == 2 && gamepad4team == 0) ||
                        (gamepad2team == 1 && gamepad3team == 2 && gamepad4team == 2) ||
                        (gamepad2team == 2 && gamepad3team == 1 && gamepad4team == 0) ||
                        (gamepad2team == 0 && gamepad3team == 1 && gamepad4team == 2) ||
                        (gamepad2team == 2 && gamepad3team == 1 && gamepad4team == 2) ||
                        (gamepad2team == 2 && gamepad3team == 0 && gamepad4team == 1) ||
                        (gamepad2team == 0 && gamepad3team == 2 && gamepad4team == 1) ||
                        (gamepad2team == 2 && gamepad3team == 2 && gamepad4team == 1))
                    {
                        continueMessage.SetActive(true);
                        if (this.gamepadState.Buttons.Start == ButtonState.Pressed)
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
                else if (gamepad1team == 2)
                {
                    if (
                        (gamepad2team == 0 && gamepad3team == 0 && gamepad4team == 0) ||
                        (gamepad2team == 2 && gamepad3team == 0 && gamepad4team == 0) ||
                        (gamepad2team == 0 && gamepad3team == 2 && gamepad4team == 0) ||
                        (gamepad2team == 0 && gamepad3team == 0 && gamepad4team == 2) ||
                        (gamepad2team == 1 && gamepad3team == 0 && gamepad4team == 0) ||
                        (gamepad2team == 0 && gamepad3team == 1 && gamepad4team == 0) ||
                        (gamepad2team == 0 && gamepad3team == 0 && gamepad4team == 1) ||
                        (gamepad2team == 1 && gamepad3team == 1 && gamepad4team == 0) ||
                        (gamepad2team == 1 && gamepad3team == 0 && gamepad4team == 1) ||
                        (gamepad2team == 0 && gamepad3team == 1 && gamepad4team == 1) ||
                        (gamepad2team == 2 && gamepad3team == 1 && gamepad4team == 0) ||
                        (gamepad2team == 2 && gamepad3team == 0 && gamepad4team == 1) ||
                        (gamepad2team == 2 && gamepad3team == 1 && gamepad4team == 1) ||
                        (gamepad2team == 1 && gamepad3team == 2 && gamepad4team == 0) ||
                        (gamepad2team == 0 && gamepad3team == 2 && gamepad4team == 1) ||
                        (gamepad2team == 1 && gamepad3team == 2 && gamepad4team == 1) ||
                        (gamepad2team == 1 && gamepad3team == 0 && gamepad4team == 2) ||
                        (gamepad2team == 0 && gamepad3team == 1 && gamepad4team == 2) ||
                        (gamepad2team == 1 && gamepad3team == 1 && gamepad4team == 2))
                    {
                        continueMessage.SetActive(true);
                        if (this.gamepadState.Buttons.Start == ButtonState.Pressed)
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
            } else {
                continueMessage.SetActive(false);
                alerteMessage.SetActive(false);
            }

            if(this.gamepadState.Buttons.Back == ButtonState.Pressed && pressedBack == true) {
                canvas.SetActive(false);
                fade.SetActive(false);
                fade.SetActive(true);
                pressedBack = false;
                StartCoroutine(CoroutineUtils.DelaySeconds(() => {
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
        }

        // for debug , appuyer sur espace pour passer directement au menu suivant avec les manettes pre-valider

        if(Input.GetKeyDown(KeyCode.Space)) {
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
        // ---------------------- Validation IA with Y ----------------------------------------

        if (gamepad1Validated == true && this.gamepadState1.Buttons.Y == ButtonState.Pressed)
        {
            btn1Y = true;
            print("Validate Gamepad 2");
            gamepad2Validated = true;
            GameObject.Find("gamepad2").GetComponent<SpriteRenderer>().sprite = whiteGamepad;

            print("Validate Gamepad 3");
            gamepad3Validated = true;
            GameObject.Find("gamepad3").GetComponent<SpriteRenderer>().sprite = whiteGamepad;

            print("Validate Gamepad 4");
            gamepad4Validated = true;
            GameObject.Find("gamepad4").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
        }

        TData savedData = FindObjectOfType<TData>();

        savedData.p1 = gamepad1team;
        savedData.p2 = gamepad2team;
        savedData.p3 = gamepad3team;
        savedData.p4 = gamepad4team;

        DontDestroyOnLoad(savedData.gameObject);
    }
}

