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
    public bool pressedStart;

    [SerializeField]
    public bool pressedA;

    [SerializeField]
    public GameObject pauseMenuUI;

    [SerializeField]
    private GamePadState gamepadState;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIndex index = (PlayerIndex.One);
        this.gamepadState = GamePad.GetState(index);

        if (this.gamepadState.Buttons.Start == ButtonState.Pressed)
     {
            Debug.Log("Pressed Start");
        if (GameIsPaused)
        {
                Resume();
        }
        else
                    
        {
                Pause();
        }
     }
        
    }

    void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
