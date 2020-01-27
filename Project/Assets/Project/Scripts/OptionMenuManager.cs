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
    bool backReleased = true;
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
    [SerializeField]
    public GameObject Fade;




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
        if (this.gamepadState.Buttons.Back == ButtonState.Pressed && backReleased == true)
        {
            Fade.SetActive(false);
            Fade.SetActive(true);
            StartCoroutine(WaitMainMenu());
            StartCoroutine(WaitFadeOut());
            actualVertical = PositionVertical.One;
            downReleased = false;
            upReleased = false;
        }
    }

    IEnumerator WaitFadeOut()
    {
        yield return new WaitForSeconds(0.7f);
        menuOption.SetActive(false);
    }

    IEnumerator WaitMainMenu()
    {
        yield return new WaitForSeconds(0.6f);
        menuPrincipal.SetActive(true);
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
        switch(GameParameter.Difficulty)
        {
            case DifficultyEnum.Easy:
                GameParameter.Difficulty = DifficultyEnum.Medium;
                easy.SetActive(false);
                medium.SetActive(true);
                break;
            case DifficultyEnum.Medium:
                GameParameter.Difficulty = DifficultyEnum.Hard;
                medium.SetActive(false);
                hard.SetActive(true);
                break;
            case DifficultyEnum.Hard:
                GameParameter.Difficulty = DifficultyEnum.Easy;
                hard.SetActive(false);
                easy.SetActive(true);
                break;
        }
        PlayerPrefs.SetInt("Diffculty", (int)GameParameter.Difficulty);
    }

    void ChangeLanguage()
    {
        switch (GameParameter.Language)
        {
            case LanguageEnum.English:
                GameParameter.Language = LanguageEnum.French;
                English.SetActive(false);
                French.SetActive(true);
                break;
            case LanguageEnum.French:
                GameParameter.Language = LanguageEnum.Spanish;
                French.SetActive(false);
                Spanish.SetActive(true);
                break;
            case LanguageEnum.Spanish:
                GameParameter.Language = LanguageEnum.German;
                Spanish.SetActive(false);
                German.SetActive(true);
                break;
            case LanguageEnum.German:
                GameParameter.Language = LanguageEnum.English;
                German.SetActive(false);
                English.SetActive(true);
                break;
        }
        PlayerPrefs.SetInt("Language", (int)GameParameter.Language);
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
        backReleased = this.gamepadState.Buttons.Back == ButtonState.Released;
    }
}