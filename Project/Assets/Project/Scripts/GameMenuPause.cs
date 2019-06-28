using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using Prime31;
using UnityEngine.SceneManagement;
using System;


public class GameMenuPause : MonoBehaviour {

    private static bool GameIsPaused = false;
    private static bool GameIsAlreadyInPaused = false;

    [SerializeField]
    public GameObject pauseMenuUI;

    [SerializeField]
    private GamePadState gamepadState;

    [SerializeField]
    public bool pressedUp;

    [SerializeField]
    public bool pressedDown;

    [SerializeField]
    public bool pressedA;

    [SerializeField]
    public bool pressedStart;

    private int Position = 0;
    private int CountPressStart = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIndex index = (PlayerIndex.One);
        this.gamepadState = GamePad.GetState(index);
        // GameIsAlreadyInPaused = false;

    //Apparition & disparition du menu en appuyant sur start

            if (this.gamepadState.Buttons.Start == ButtonState.Pressed)
        {
            CountPressStart++;
            pressedStart = false;
            if (CountPressStart<2)
            {
                if (GameIsAlreadyInPaused)
                {
                    GameIsPaused = false;
                    Debug.Log("Return Game");
                    pauseMenuUI.SetActive(false);
                    Time.timeScale = 1f;
                    GameIsAlreadyInPaused = false;
                }
                else
                {
                    Debug.Log("Pause");
                    pauseMenuUI.SetActive(true);
                    Time.timeScale = 0f;
                    GameIsAlreadyInPaused = true;
                }
            }
        }

        else if (this.gamepadState.Buttons.Start == ButtonState.Released)
        {
            pressedStart = true;
            CountPressStart = 0;
        }

            //Interractions avec le menu

        if(this.gamepadState.DPad.Up == ButtonState.Pressed && pauseMenuUI == true)
        {
            Position = 0;
            Debug.Log(Position);
        }

        if(this.gamepadState.DPad.Down == ButtonState.Pressed && pauseMenuUI == true)
        {
            Position = 3;
            Debug.Log(Position);
        }

            if (this.gamepadState.Buttons.A == ButtonState.Pressed && pauseMenuUI == true && Position == 0)
        {
            print("Resume");
            GameIsPaused = false;
            Debug.Log("Return Game");
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsAlreadyInPaused = false;
        } 
            
            if (this.gamepadState.Buttons.A == ButtonState.Pressed && pauseMenuUI == true && Position == 3)
        {
            print("QuitGame");
            SceneManager.LoadScene("_mainmenu");
        }  
    }


    //Action du menu

    public void SwitchBetweenPause()
    {

    }

    public void Restart()
    {
        Position = 1;
        Debug.Log("Restart");
    }

     public void Option()
    {
        Position = 2;
        Debug.Log("Option");
    }

    public void QuitGame()
    {
        Position = 3;
        Debug.Log("Quit");
    }
}
