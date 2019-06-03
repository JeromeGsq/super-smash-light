using UnityEngine;
using Prime31;
using GamepadInput;
using static GamepadInput.ip_GamePad;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameMenuManager2 : MonoBehaviour {

    static public int gamepad1team;
    static public int gamepad2team;
    static public int gamepad3team;
    static public int gamepad4team;

    static public int gamepad1color;
    static public int gamepad2color;
    static public int gamepad3color;
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

    private GamepadState gamepadState1;
    private GamepadState gamepadState2;
    private GamepadState gamepadState3;
    private GamepadState gamepadState4;
    private GamepadState gamepadStateAny;

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

        this.gamepadState1 = new GamepadState();
        this.gamepadState2 = new GamepadState();
        this.gamepadState3 = new GamepadState();
        this.gamepadState4 = new GamepadState();
        this.gamepadStateAny = new GamepadState();
        

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

        ip_GamePad.GetState(ref this.gamepadState1, ip_GamePad.Index.One);
        ip_GamePad.GetState(ref this.gamepadState2, ip_GamePad.Index.Two);
        ip_GamePad.GetState(ref this.gamepadState3, ip_GamePad.Index.Three);
        ip_GamePad.GetState(ref this.gamepadState4, ip_GamePad.Index.Four);
        ip_GamePad.GetState(ref this.gamepadStateAny, ip_GamePad.Index.Any);
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
            GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = new Color(1,1,1);
        }
        if(gamepad4color == 0) {
            GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = new Color(1,1,1);
        }
        if(gamepad2color == 0) {
            GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = new Color(1,1,1);
        }
        if(gamepad1color == 0) {
            GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = new Color(1,1,1);
        }
        if(this.gamepad1Validated == false)
        {

            if(this.gamepadState1.RightPressed) {
                gamepad1team = 2;
                if(gamepad1color >= 20) {
                    gamepad1color -= 20;
                }
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(8.33f, 0, -2.3f);
            }
            if(this.gamepadState1.LeftPressed) {
                gamepad1team = 1;
                if(gamepad1color <= 21) {
                    gamepad1color += 20;
                }
                gamepad1.GetComponent<Transform>().localPosition = new Vector3(-8.33f, 0, -2.3f);
            }
        }
        if(gamepad1team != 0 && this.gamepad1Validated == false) {

            if(this.gamepadState1.UpPressed) {
                gamepad1color += 1;
            }
            if(gamepad1team == 2) {

                if(gamepadState1.APressed && gamepad1color != 0) {
                    gamepad1Validated = true;
                    GameObject.Find("gamepad1").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                }

                if(gamepad1color > 4 && gamepad1color <= 5) {
                    gamepad1color = 1;
                }
            }
            if(gamepad1team == 1) {

                if(gamepadState1.APressed && gamepad1color !=20) {
                    gamepad1Validated = true;
                    GameObject.Find("gamepad1").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
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
        if(this.gamepad1Validated == true) 
        {
            if(this.gamepadState1.BPressed) 
            {
                GameObject.Find("gamepad1").GetComponent<SpriteRenderer>().sprite = greyGamepad;
                this.gamepad1Validated = false;
            }
        }


        if(this.gamepad2Validated == false) 
        {

            if(this.gamepadState2.RightPressed) {
                gamepad2team = 2;
                if(gamepad2color >= 20) {
                    gamepad2color -= 20;
                }
                gamepad2.GetComponent<Transform>().localPosition = new Vector3(8.33f, -2.5f, -2.3f);
            }
            if(this.gamepadState2.LeftPressed) {
                gamepad2team = 1;
                if(gamepad2color <= 21) {
                    gamepad2color += 20;
                }
                gamepad2.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -2.5f, -2.3f);
            }
        }
        if(gamepad2team != 0 && this.gamepad2Validated == false) {


            if(this.gamepadState2.UpPressed) {
                gamepad2color += 1;

            }
            if(gamepad2team == 2) {

                if(gamepadState2.APressed && gamepad2color != 0) {
                    gamepad2Validated = true;
                    GameObject.Find("gamepad2").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                }

                if(gamepad2color > 4 && gamepad2color <= 5) {
                    gamepad2color = 1;
                }
            }

            if(gamepad2team == 1) {

                if(gamepadState2.APressed && gamepad2color != 20) {
                    gamepad2Validated = true;
                    GameObject.Find("gamepad2").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                }

                if(gamepad2color > 24 & gamepad2color <= 25) {
                    gamepad2color = 21;
                }
            }
            if(gamepad4team == gamepad2team) {
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
        if(this.gamepad2Validated == true) 
            {
            if(this.gamepadState2.BPressed) 
            {
                GameObject.Find("gamepad2").GetComponent<SpriteRenderer>().sprite = greyGamepad;
                this.gamepad2Validated = false;
            }
        }


        if(this.gamepad3Validated == false) 
        {

            if(this.gamepadState3.RightPressed) {
                gamepad3team = 2;
                if(gamepad3color >= 20) {
                    gamepad3color -= 20;
                }
                gamepad3.GetComponent<Transform>().localPosition = new Vector3(8.33f, -5f, -2.3f);
            }
            if(this.gamepadState3.LeftPressed) {
                gamepad3team = 1;
                if(gamepad3color <= 21) {
                    gamepad3color += 20;
                }
                gamepad3.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -5f, -2.3f);
            }
        }
        if(gamepad3team != 0 && this.gamepad3Validated == false) {

            if(this.gamepadState3.UpPressed) {
                gamepad3color += 1;
            }
            if(gamepad3team == 2) {

                if(gamepadState3.APressed && gamepad3color != 0) {
                    gamepad3Validated = true;
                    GameObject.Find("gamepad3").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                }

                if(gamepad3color > 4 && gamepad3color <= 5) {
                    gamepad3color = 1;
                }

            }
            if(gamepad3team == 1) {

                if(gamepadState3.APressed && gamepad3color != 20) {
                    gamepad3Validated = true;
                    GameObject.Find("gamepad3").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
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
        if(this.gamepad3Validated == true) 
        {
            if(this.gamepadState3.BPressed) 
            {
                GameObject.Find("gamepad3").GetComponent<SpriteRenderer>().sprite = greyGamepad;
                this.gamepad3Validated = false;
            }
        }


        if(this.gamepad4Validated == false) 
        {

            if(this.gamepadState4.RightPressed) {
                gamepad4team = 2;
                if(gamepad4color >= 20) {
                    gamepad4color -= 20;
                }
                gamepad4.GetComponent<Transform>().localPosition = new Vector3(8.33f, -7.5f, -2.3f);
            }
            if(this.gamepadState4.LeftPressed) {
                gamepad4team = 1;
                if(gamepad4color <= 21) {
                    gamepad4color += 20;
                }
                gamepad4.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -7.5f, -2.3f);
            }
        }
        if(gamepad4team != 0 && this.gamepad4Validated == false) {

            if(this.gamepadState4.UpPressed) {
                gamepad4color += 1;
            }

            if(gamepad4team == 2) {

                if(gamepadState4.APressed && gamepad4color != 0) {
                    gamepad4Validated = true;
                    GameObject.Find("gamepad4").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
                }

                if(gamepad4color > 4 && gamepad4color <= 5) {
                    gamepad4color = 1;
                }
            }
            if(gamepad4team == 1) {

                if(gamepadState4.APressed && gamepad4color != 20) {
                    gamepad4Validated = true;
                    GameObject.Find("gamepad4").GetComponent<SpriteRenderer>().sprite = whiteGamepad;
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
            if(this.gamepadState4.BPressed) {
                GameObject.Find("gamepad4").GetComponent<SpriteRenderer>().sprite = greyGamepad;
                this.gamepad4Validated = false;
            }
        }

        if(gamepad1Validated && gamepad2Validated && gamepad3Validated && gamepad4Validated) {
            if((gamepad1team + gamepad2team + gamepad3team + gamepad4team) == 6) {
                continueMessage.SetActive(true);
                if(this.gamepadStateAny.Start) {
                    this.gameObject.SetActive(false);
                    Menu3.SetActive(true);
                }
            }
            if((gamepad1team + gamepad2team + gamepad3team + gamepad4team) != 6) {
                alerteMessage.SetActive(true);
            }
        } 
        else 
        {
            continueMessage.SetActive(false);
            alerteMessage.SetActive(false);
        }

        if(gamepadStateAny.Back) {
            canvas.SetActive(false);
            fade.SetActive(false);
            fade.SetActive(true);
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
}

