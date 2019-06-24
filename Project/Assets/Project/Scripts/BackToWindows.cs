using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using Prime31;
using UnityEngine.SceneManagement;
using System;

public class BackToWindows : MonoBehaviour
{

    [SerializeField]
    public GameObject BoutonQuitter;


    [SerializeField]
    private GamePadState gamepadState;

    [SerializeField]
    public GameObject MenuPrincipal;

    //[SerializeField]
    //public GameObject AnimatedBandeau;

    [SerializeField]
    public bool pressedLeft;
    [SerializeField]
    public bool pressedRight;
    [SerializeField]
    public bool pressedA;
    //[SerializeField]
    //public bool pressedB;

    [SerializeField]
    public GameObject returnWindows;
    [SerializeField]
    public GameObject cursorPosYes;

    [SerializeField]
    public bool stateYes;

    [SerializeField]
    public GameObject cursorPosNo;

    [SerializeField]
    public bool stateNo;

    [SerializeField]
    public ButtonState LeftStick { get; }

    [SerializeField]
    public GameObject fadeQuit;


    // Start is called before the first frame update
    void Start()
    {
        stateNo = true;
    }

    // Update is called once per frame
    void Update()
    {
        // for(int i = 0; i < 4; ++i) {
        PlayerIndex index = (PlayerIndex.One);
        this.gamepadState = GamePad.GetState(index);
        if (this.gamepadState.DPad.Left == ButtonState.Pressed)
            {
            stateYes = true;
            stateNo = false;
            cursorPosYes.SetActive(true);
            cursorPosNo.SetActive(false);
            Debug.Log("left");
            }

        if (this.gamepadState.DPad.Right == ButtonState.Pressed)
        {
            stateNo = true;
            stateYes = false;
            cursorPosNo.SetActive(true);
            cursorPosYes.SetActive(false);
            Debug.Log("right");
        }

        if(stateYes == true && cursorPosYes == true && this.gamepadState.Buttons.A == ButtonState.Pressed)
        {
            returnWindows.SetActive(true);
            MenuPrincipal.SetActive(false);
            Debug.Log("Quit");
            fadeQuit.SetActive(true);
            Application.Quit();

#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
#endif

        }

        if (stateNo == true && cursorPosNo == true && this.gamepadState.Buttons.A == ButtonState.Pressed)
        {
            fadeQuit.SetActive(false);
            returnWindows.SetActive(false);
            MenuPrincipal.SetActive(true);
            Debug.Log("Retour menu");
            

        }
    }
}