using System;
using GamepadInput;
using UnityEngine;
using UnityWeld.Binding;
using static GamepadInput.ip_GamePad;
using UnityEngine.SceneManagement;

public class EndOfGameScript : MonoBehaviour
{

    private GamepadState gamepadState;

    // Start is called before the first frame update
    void Start()
    {
        this.gamepadState = new GamepadState();
    }

    // Update is called once per frame
    void Update()
    {
        ip_GamePad.GetState(ref this.gamepadState, ip_GamePad.Index.Any);

        if(this.gamepadState.Start) {
            SceneManager.LoadScene(1);
        }
        if(this.gamepadState.Back) {
            SceneManager.LoadScene(0);
        }
    }
}
