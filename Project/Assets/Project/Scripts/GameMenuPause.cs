using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using Prime31;
using UnityEngine.SceneManagement;
using System;


public class GameMenuPause : MonoBehaviour {

    private static bool GameIsPaused = false;
   // private static bool GameIsAlreadyInPaused = false;

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
    public bool pressedStart = true;

    private int Position = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIndex index = (PlayerIndex.One);
        this.gamepadState = GamePad.GetState(index);


        if (this.gamepadState.Buttons.Start == ButtonState.Pressed && pressedStart == true)
        {
            pressedStart = false;
            if (GameIsPaused)
            {
                Debug.Log("Return Game");
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                GameIsPaused = false;
            }
            else
            {
                //GameIsPaused = true;
                Debug.Log("Pause");
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0f;
                GameIsPaused = true;
            }
        }

        if (this.gamepadState.Buttons.Start == ButtonState.Pressed )
        {
            pressedStart = true;
        }

        //if (this.gamepadState.Buttons.Start == ButtonState.Released)
        //{
        //    GameIsPaused = GameIsPaused == true ? false : true;
        //}

        //if (this.gamepadState.Buttons.Start == ButtonState.Pressed && GameIsAlreadyInPaused == false)
        //{
        //    GameIsPaused = true;
        //    pressedStart = true;
        //}

        //if (this.gamepadState.Buttons.Start == ButtonState.Released && GameIsPaused == true && pressedStart == true && GameIsAlreadyInPaused == false)
        //{
        //    Debug.Log("Pause");
        //    pauseMenuUI.SetActive(true);
        //    Time.timeScale = 0f;
        //    GameIsAlreadyInPaused = true;
        //}

        //if (this.gamepadState.Buttons.Start == ButtonState.Pressed && GameIsPaused == true  && GameIsAlreadyInPaused == true)
        //{
        //    GameIsPaused = false;
        //    pressedStart = true;
        //}

        // if (this.gamepadState.Buttons.Start == ButtonState.Released && GameIsPaused == false && pressedStart == true && GameIsAlreadyInPaused == true)
        //{
        //    Debug.Log("Return Game");
        //    pauseMenuUI.SetActive(false);
        //    Time.timeScale = 1f;
        //    GameIsAlreadyInPaused = false;
        //}

        //pressedStart = false;
        //Time.timeScale = 1;
        //GameIsPaused = false;
        //pauseMenuUI.SetActive(false);



        //if (this.gamepadState.Buttons.Start == ButtonState.Released && pressedStart == true && GameIsPaused == true)
        //{
        //    pressedStart = false;
        //    Time.timeScale = 0;
        //    GameIsPaused = true;
        //    pauseMenuUI.SetActive(true);
        //}

        //if (this.gamepadState.Buttons.Back == ButtonState.Released && pressedStart == false)
        //{
        //    pressedStart = false;
        //    Time.timeScale = 1;
        //    GameIsPaused = false;
        //    pauseMenuUI.SetActive(false);
        //}


        //   if (this.gamepadState.Buttons.Start == ButtonState.Pressed && GameIsPaused == true)
        //{
        //}


        //if (GameIsPaused)
        //{
        //        Resume();
        //}
        //else

        //{
        //        Pause();
        //}
    }

    //public void Resume ()
    //{
    //}

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
