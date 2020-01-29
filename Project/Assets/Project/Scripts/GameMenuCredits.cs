using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using Prime31;
using UnityEngine.SceneManagement;
using System;

public class GameMenuCredits : MonoBehaviour
{
    [SerializeField]
    private GamePadState gamepadState;

    bool backReleased = true;

    [SerializeField]
    public GameObject menuOption;
    [SerializeField]
    public GameObject menuCredits;
    [SerializeField]
    public GameObject Fade;
    [SerializeField]
    public GameObject FadeTextCredits;

    // Start is called before the first frame update
    void Start()
    {
        FadeTextCredits.SetActive(true);
    }

    void GetInputBack()
    {
        if (this.gamepadState.Buttons.Back == ButtonState.Pressed && backReleased == true)
        {
            Fade.SetActive(false);
            Fade.SetActive(true);
            StartCoroutine(WaitIOptionMenu());
            StartCoroutine(WaitFadeOut());
        }
    }

    IEnumerator WaitFadeOut()
    {
        yield return new WaitForSeconds(0.7f);
        menuCredits.SetActive(false);
    }

    IEnumerator WaitIOptionMenu()
    {
        yield return new WaitForSeconds(0.6f);
        menuOption.SetActive(true);
        FadeTextCredits.SetActive(false);

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
        GetInputBack();
        backReleased = this.gamepadState.Buttons.Back == ButtonState.Released;
    }
}
