using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using Prime31;
using UnityEngine.SceneManagement;
using System;

public class OptionMenuManager : MonoBehaviour
{
    private int Vertical = 0;

    [SerializeField]
    private GamePadState gamepadState;

    public bool pressedA;

    bool dpadUp;
    bool dpadDown = false;

    [SerializeField]
    private GameObject easy;

    [SerializeField]
    private GameObject medium;

    [SerializeField]
    private GameObject hard;

    

    [SerializeField]
    public GameObject selecteurPos1;

    [SerializeField]
    public GameObject selecteurPos2;

    [SerializeField]
    public GameObject selecteurPos3;

  

   // Start is called before the first frame update
    void Start()
    {
        
        easy.SetActive(true);
        selecteurPos1.SetActive(true);
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            this.gamepadState = GamePad.GetState((PlayerIndex)i);
            if (gamepadState.IsConnected)
            {
                break;
            }
        }
        //dpadDown = ;
        if (this.gamepadState.DPad.Down == ButtonState.Pressed)
        {
            Debug.Log("Test");
            switch (Vertical)
            {
                case 0:
                    selecteurPos1.SetActive(true);
                    Vertical = 1;
                    break;
                case 1:
                    selecteurPos1.SetActive(false);
                    selecteurPos2.SetActive(true);
                    Vertical = 2;
                    break;
                case 2:
                    selecteurPos2.SetActive(false);
                    selecteurPos3.SetActive(true);
                    break;
                default:
                    throw new Exception("Vertical Position is not valid: " + Vertical);
            }
        }

        dpadUp = this.gamepadState.DPad.Up == ButtonState.Pressed;
        if (dpadUp)
        {
            switch (Vertical)
            {
                case 0:
                    selecteurPos3.SetActive(true);
                    selecteurPos2.SetActive(false);
                    Vertical = 2;
                    break;
                case 1:
                    selecteurPos3.SetActive(false);
                    selecteurPos2.SetActive(true);
                    Vertical = 1;
                    break;
                case 2:
                    selecteurPos2.SetActive(false);
                    selecteurPos1.SetActive(true);
                    break;
                default:
                    throw new Exception("Vertical Position is not valid: " + Vertical);
            }
        }

        //if (dpadDown == true && Vertical == 0)
        //{
        //    selecteurPos1.SetActive(true);
        //    Vertical = 1;
        //}

        //else if (dpadDown == true && Vertical == 1)
        //{
        //    selecteurPos1.SetActive(false);
        //    selecteurPos2.SetActive(true);
        //    Vertical = 2;
        //}

        //else if (dpadDown == true && Vertical == 2)
        //{
        //    selecteurPos2.SetActive(false);
        //    selecteurPos3.SetActive(true);
        //}
        ////Pour pas compter l'appui d'un bouton plusieurs frames de suite
        //if (this.gamepadState1.DPad.Up == ButtonState.Released)
        //{
        //    dpadUp = false;
        //}


        //if (gamepadState1.Buttons.A == ButtonState.Released) g1a = false;


        ////if (vertical >= 3 gamepadState1.Buttons.A == ButtonState.Pressed)
        //{

        //}
        ////    if (this.gamepadState1.DPad.Down == ButtonState.Pressed)
        ////{
        ////    Vertical = -1;    
        ////}


        //if (Vertical == -1)
        //{
        //    selecteurPos1.SetActive(false);
        //    selecteurPos2.SetActive(true);
        //    //selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -1.25f, selecteur.GetComponent<Transform>().position.z);
        //}

        Debug.Log(Vertical);
       // Debug.Log(dpadDown);
        Debug.Log("Connceted Update"+ gamepadState.IsConnected);

    }
}