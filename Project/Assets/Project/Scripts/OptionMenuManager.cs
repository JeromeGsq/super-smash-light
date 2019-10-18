using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using Prime31;
using UnityEngine.SceneManagement;
using System;

public class OptionMenuManager : MonoBehaviour
{
    [SerializeField]
    private int Horizontal = 0;

    [SerializeField]
    private int Vertical = 0;

    [SerializeField]
    private bool Dpad_Down;

    [SerializeField]
    private bool Dpad_Up;


    [SerializeField]
    public GameObject Easy;

    [SerializeField]
    public GameObject medium;

    [SerializeField]
    public GameObject hard;

    [SerializeField]
    private GamePadState gamepadState;

    [SerializeField]
    public GameObject selecteurPos1;

    [SerializeField]
    public GameObject selecteurPos2;

    [SerializeField]
    public GameObject selecteurPos3;

    //[SerializeField]
    //public int PositionV = 1;

   // Start is called before the first frame update
    void Start()
    {
        Easy.SetActive(true);
        selecteurPos1.SetActive(true);
        Dpad_Down = false;
    }

    // Update is called once per frame
    void Update()
    {
            if (this.gamepadState.DPad.Down == ButtonState.Pressed)
            {
                Dpad_Down = true;

            }
                if(Dpad_Down == true)
            {
                Vertical = 1;
                selecteurPos1.SetActive(false);
                selecteurPos2.SetActive(true);
                Debug.Log(Vertical);
                //selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -1.25f, selecteur.GetComponent<Transform>().position.z);
            }
        

        if (Horizontal == 1)
        {
            if (this.gamepadState.DPad.Left == ButtonState.Pressed)
            {
                Easy.SetActive(false);
                medium.SetActive(true);
            }
            
        }


        //if(this.gamepadState.Dpad.Down == ButtonState.Pressed && PositionH == 1)
        //{
        //    PositionH = 2;
        //    selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -1.25f, selecteur.GetComponent<Transform>().position.z);
        //}
    }
}