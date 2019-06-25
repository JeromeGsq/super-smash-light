using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using Prime31;
using UnityEngine.SceneManagement;
using System;


public class GameMenuPause : MonoBehaviour {

    public static bool GameIsPaused = false;

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
    public bool pressedSelect;

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

        if (this.gamepadState.Buttons.Back == ButtonState.Pressed)
        {
            Debug.Log("Pressed Start");
            pressedSelect = true;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0.0001f;
            GameIsPaused = true;

        }

        if (this.gamepadState.Buttons.Back == ButtonState.Released && pressedSelect == true && GameIsPaused == true)
        {
            pressedSelect = false;
            Time.timeScale = 0;
            GameIsPaused = true;
            pauseMenuUI.SetActive(true);
        }

        if (this.gamepadState.Buttons.Back == ButtonState.Released && pressedSelect == false)
        {
            pressedSelect = false;
            Time.timeScale = 1;
            GameIsPaused = false;
            pauseMenuUI.SetActive(false);
        }
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

     public void Option()
    {
        Position = 1;
        Debug.Log("Option");
    }

    public void QuitGame()
    {
        Position = 2;
        Debug.Log("Quit");
    }
}
