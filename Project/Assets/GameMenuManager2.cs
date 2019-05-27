using UnityEngine;
using Prime31;
using GamepadInput;
using static GamepadInput.ip_GamePad;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameMenuManager2 : MonoBehaviour {


    public int gamepad1team;
    public int gamepad2team;
    public int gamepad3team;
    public int gamepad4team;

    public int gamepad1color;
    public int gamepad2color;
    public int gamepad3color;
    public int gamepad4color;

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


    // Start is called before the first frame update
    void Start() {

        gamepad1team = 1;
        gamepad2team = 1;
        gamepad3team = 2;
        gamepad4team = 2;

        gamepad1color = 0;
        gamepad2color = 0;
        gamepad3color = 0;
        gamepad4color = 0;

        this.gamepadState1 = new GamepadState();
        this.gamepadState2 = new GamepadState();
        this.gamepadState3 = new GamepadState();
        this.gamepadState4 = new GamepadState();

    }

    // Update is called once per frame
    void Update() {

        ip_GamePad.GetState(ref this.gamepadState1, ip_GamePad.Index.One);
        ip_GamePad.GetState(ref this.gamepadState2, ip_GamePad.Index.Two);
        ip_GamePad.GetState(ref this.gamepadState3, ip_GamePad.Index.Three);
        ip_GamePad.GetState(ref this.gamepadState4, ip_GamePad.Index.Four);


        if(this.gamepadState1.RightPressed && gamepad1team == 1) {
            gamepad1team = 2;
            if(gamepad1color >= 20) 
            {
                gamepad1color -= 20;
            }
            gamepad1.GetComponent<Transform>().localPosition = new Vector3(8.33f, 0, -2.3f);
        }
        if(this.gamepadState1.LeftPressed && gamepad1team == 2) {
            gamepad1team = 1;
            if(gamepad1color <=21) {
                gamepad1color += 20;
            }
            gamepad1.GetComponent<Transform>().localPosition = new Vector3(-8.33f, 0, -2.3f);
        }


        if(this.gamepadState2.RightPressed && gamepad2team == 1) {
            gamepad2team = 2;
            if(gamepad2color >= 20) 
            {
                gamepad2color -= 20;
            }
            gamepad2.GetComponent<Transform>().localPosition = new Vector3(8.33f, -2.5f, -2.3f);
        }
        if(this.gamepadState2.LeftPressed && gamepad2team == 2) {
            gamepad2team = 1;
            if(gamepad2color <= 21) {
                gamepad2color += 20;
            }
            gamepad2.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -2.5f, -2.3f);
        }


        if(this.gamepadState3.RightPressed && gamepad3team == 1) {
            gamepad3team = 2;
            if(gamepad3color >= 20) {
                gamepad3color -= 20;
            }
            gamepad3.GetComponent<Transform>().localPosition = new Vector3(8.33f, -5f, -2.3f);
        }
        if(this.gamepadState3.LeftPressed && gamepad3team == 2) {
            gamepad3team = 1;
            if(gamepad3color <= 21) {
                gamepad3color += 20;
            }
            gamepad3.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -5f, -2.3f);
        }



        if(this.gamepadState4.RightPressed && gamepad4team == 1) {
            gamepad4team = 2;
            if(gamepad4color >= 20) {
                gamepad4color -= 20;
            }
            gamepad4.GetComponent<Transform>().localPosition = new Vector3(8.33f, -7.5f, -2.3f);
        }
        if(this.gamepadState4.LeftPressed && gamepad4team == 2) {
            gamepad4team = 1;
            if(gamepad4color <= 21) {
                gamepad4color += 20;
            }
            gamepad4.GetComponent<Transform>().localPosition = new Vector3(-8.33f, -7.5f, -2.3f);
        }

        if(gamepad3team != 0) {

            if(this.gamepadState3.UpPressed) {
                gamepad3color += 1;
            }
            if(gamepad3team == 2) {

                if(gamepad3color > 4 && gamepad3color <= 5) {
                    gamepad3color = 1;
                }

            }
            if(gamepad3team == 1) {

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
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }
            if(gamepad3color == 2) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
            }
            if(gamepad3color == 3) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
            }
            if(gamepad3color == 4) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = new Color(100f, 100f, 255f);
            }
            if(gamepad3color == 21) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = new Color(200, 200, 0);
            }
            if(gamepad3color == 22) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = new Color(0, 200, 200);
            }
            if(gamepad3color == 23) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = new Color(200, 0, 200);
            }
            if(gamepad3color == 24) {
                GameObject.Find("gamepadColor3").GetComponent<SpriteRenderer>().color = new Color(200f, 50f, 200f);
            }

        }
        if(gamepad4team != 0) {

            if(this.gamepadState4.UpPressed) 
            {
                gamepad4color += 1;
            }
            
            if(gamepad4team == 2) {

                if(gamepad4color > 4 && gamepad4color <= 5) {
                    gamepad4color = 1;
                }
            }
        if(gamepad4team == 1) {

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
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }
            if(gamepad4color == 2) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
            }
            if(gamepad4color == 3) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
            }
            if(gamepad4color == 4) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = new Color(100, 100, 255);
            }
            if(gamepad4color == 21) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = new Color(200, 200, 0);
            }
            if(gamepad4color == 22) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = new Color(0, 200, 200);
            }
            if(gamepad4color == 23) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = new Color(200, 0, 200);
            }
            if(gamepad4color == 24) {
                GameObject.Find("gamepadColor4").GetComponent<SpriteRenderer>().color = new Color(200, 50, 200);
            }
        }
        if(gamepad2team != 0) {

            if(this.gamepadState2.UpPressed) 
            {
                gamepad2color += 1;

            }
            if(gamepad2team == 2)
            {
                if(gamepad2color > 4 && gamepad2color <= 5) 
                {
                    gamepad2color = 1;
                }
            }

        if(gamepad2team == 1) 
        {
            if(gamepad2color > 24 & gamepad2color <= 25) 
            {
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
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }
            if(gamepad2color == 2) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
            }
            if(gamepad2color == 3) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
            }
            if(gamepad2color == 4) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = new Color(100, 100, 255);
            }
            if(gamepad2color == 21) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = new Color(200, 200, 0);
            }
            if(gamepad2color == 22) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = new Color(0, 200, 200);
            }
            if(gamepad2color == 23) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = new Color(200, 0, 200);
            }
            if(gamepad2color == 24) {
                GameObject.Find("gamepadColor2").GetComponent<SpriteRenderer>().color = new Color(200, 50, 200);
            }
        }
        if(gamepad1team != 0) {

            if(this.gamepadState1.UpPressed) {
                gamepad1color += 1;
            }
            if(gamepad1team == 2) {

                if(gamepad1color > 4 && gamepad1color <= 5) {
                    gamepad1color = 1;
                }
            }
                if(gamepad1team == 1) {

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
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }
            if(gamepad1color == 2) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
            }
            if(gamepad1color == 3) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
            }
            if(gamepad1color == 4) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = new Color(100f, 100f, 255f);
            }
            if(gamepad1color == 21) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = new Color(200, 200, 0);
            }
            if(gamepad1color == 22) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = new Color(0, 200, 200);
            }
            if(gamepad1color == 23) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = new Color(200, 0, 200);
            }
            if(gamepad1color == 24) {
                GameObject.Find("gamepadColor1").GetComponent<SpriteRenderer>().color = new Color(200f, 50, 200f);
            }
        }
    }
}

