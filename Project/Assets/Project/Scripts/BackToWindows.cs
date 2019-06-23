﻿using System.Collections;
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


    // Start is called before the first frame update
    void Start()
    {
        if(returnWindows == true)
        {
            pressedLeft = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // for(int i = 0; i < 4; ++i) {
        PlayerIndex index = (PlayerIndex.One);
        if (this.gamepadState.DPad.Left == ButtonState.Pressed && pressedLeft == true)
        {
                pressedLeft = true;
            if (this.gamepadState.DPad.Left == ButtonState.Pressed && pressedLeft == true)
            {
                Debug.Log(pressedLeft);
            }

            if (pressedLeft == true && this.gamepadState.DPad.Left == ButtonState.Released)
            {
                pressedLeft = false;
            }
        }
        //if (this.gamepadState.Buttons.A == ButtonState.Released && pressedA == true)
        //{
        //    Debug.Log("Retour Windows");
        //    // BoutonQuitter.GetComponent<Animator>().Play(Animator.StringToHash("OptionMenu"));
        //    //MenuPrincipal.SetActive(true);
        //    //returnWindows.SetActive(false);
        //    //cursorPosYes.SetActive(true);
        //    //cursorPosNo.SetActive(false);
        //}

        //Application.Quit();
    }

}