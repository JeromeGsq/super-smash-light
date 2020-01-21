using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using Prime31;
using UnityEngine.SceneManagement;
using System;

public class OptionMenuManager : MonoBehaviour
{
    private int Vertical = 1;

    [SerializeField]
    private GamePadState gamepadState;

    public bool pressedA;

    bool dpadUp;
    bool dpadDown = false;
    bool DownIsPressed;
    bool DownIsAlreadyPressed = false;
    bool UpIsPressed;
    bool UpIsAlreadyPressed =false;


    
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

    private enum PositionVertical {One, Two, Three}
    private PositionVertical actualVertical = PositionVertical.One;
    private enum Difficulties { Easy, Medium, Hard}
    private Difficulties actualDifficulties = Difficulties.Easy;
    private enum Languages { English, French, Spanish, German }
    private Languages actualLanguage = Languages.English;

    bool downReleased = true;
    bool upReleased = true;
    bool aReleased = true;
    [SerializeField]
    public GameObject menuPrincipal;
    [SerializeField]
    public GameObject menuOption;
    [SerializeField]
    public GameObject English;
    [SerializeField]
    public GameObject French;
    [SerializeField]
    public GameObject Spanish;
    [SerializeField]
    public GameObject German;




    // Start is called before the first frame update
    void Start()
    {
        
        easy.SetActive(true);
        selecteurPos1.SetActive(true);
    }
    void GetInputDown()
    {
        if (this.gamepadState.DPad.Down == ButtonState.Pressed && downReleased == true)
        {
            downReleased = false;
            switch(actualVertical)
            {
                case PositionVertical.One:
                    actualVertical = PositionVertical.Two;
                    break;
                case PositionVertical.Two:
                    actualVertical = PositionVertical.Three;
                    break;
                case PositionVertical.Three:
                    actualVertical = PositionVertical.Three;
                    break;
                default:
                    throw new Exception("Vertical Position is not valid: " + Vertical);
            }
        }
    }

    void GetInputUp()
    {
        if (this.gamepadState.DPad.Up == ButtonState.Pressed && upReleased == true)
        {
            upReleased = false;
            switch (actualVertical)
            {
                case PositionVertical.One:
                    actualVertical = PositionVertical.One;
                    break;
                case PositionVertical.Two:
                    actualVertical = PositionVertical.One;
                    break;
                case PositionVertical.Three:
                    actualVertical = PositionVertical.Two;
                    break;
                default:
                    throw new Exception("Vertical Position is not valid: " + Vertical);
            }
        }
    }

    void GetInputBack()
    {
        if (this.gamepadState.Buttons.Back == ButtonState.Pressed)
        {
            actualVertical = PositionVertical.One;
            menuPrincipal.SetActive(true);
            menuOption.SetActive(false);
            downReleased = false;
            upReleased = false;


        }
    }
    void GetInputA()
    {
        if (this.gamepadState.Buttons.A == ButtonState.Pressed && aReleased == true)
        {
            aReleased = false;
            switch (actualVertical)
            {
                case PositionVertical.One:
                    ChangeDifficulty();
                    break;
                case PositionVertical.Two:
                    ChangeLanguage();
                    break;
                case PositionVertical.Three:
                    menuPrincipal.SetActive(true);
                    menuOption.SetActive(false);
                    break;
                default:
                    throw new Exception("Vertical Position is not valid: " + Vertical);
            }
        }
    }

    void ChangeDifficulty()
    {
        switch(actualDifficulties)
        {
            case Difficulties.Easy:
                actualDifficulties = Difficulties.Medium;
                easy.SetActive(false);
                medium.SetActive(true);
                break;
            case Difficulties.Medium:
                actualDifficulties = Difficulties.Hard;
                medium.SetActive(false);
                hard.SetActive(true);
                break;
            case Difficulties.Hard:
                actualDifficulties = Difficulties.Easy;
                hard.SetActive(false);
                easy.SetActive(true);
                break;

        }
    }

    void ChangeLanguage()
    {
        switch (actualLanguage)
        {
            case Languages.English:
                actualLanguage = Languages.French;
                English.SetActive(false);
                French.SetActive(true);
                break;
            case Languages.French:
                actualLanguage = Languages.Spanish;
                French.SetActive(false);
                Spanish.SetActive(true);
                break;
            case Languages.Spanish:
                actualLanguage = Languages.German;
                Spanish.SetActive(false);
                German.SetActive(true);
                break;
            case Languages.German:
                actualLanguage = Languages.English;
                German.SetActive(false);
                English.SetActive(true);
                break;
        }
    }

    void DisplayCursor()
    {
        switch (actualVertical)
        {
            case PositionVertical.One:
                selecteurPos1.SetActive(true);
                selecteurPos2.SetActive(false);
                selecteurPos3.SetActive(false);
                break;
            case PositionVertical.Two:
                selecteurPos1.SetActive(false);
                selecteurPos2.SetActive(true);
                selecteurPos3.SetActive(false);
                break;
            case PositionVertical.Three:
                selecteurPos1.SetActive(false);
                selecteurPos2.SetActive(false);
                selecteurPos3.SetActive(true);
                break;
            default:
                throw new Exception("Vertical Position is not valid: " + Vertical);

        }
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

        GetInputDown();
        GetInputUp();
        GetInputA();
        GetInputBack();
        DisplayCursor();
        downReleased = this.gamepadState.DPad.Down == ButtonState.Released;
        upReleased = this.gamepadState.DPad.Up == ButtonState.Released;
        aReleased = this.gamepadState.Buttons.A == ButtonState.Released;
    }
}