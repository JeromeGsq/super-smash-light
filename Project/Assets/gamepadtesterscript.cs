using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class gamepadtesterscript : MonoBehaviour
{
    public bool gamepad1boutonA;
    public PlayerIndex controllerNumber = PlayerIndex.One;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GamePadState state = GamePad.GetState(controllerNumber);

        if(state.Buttons.A == ButtonState.Pressed) {
            gamepad1boutonA = true;
        }

        if(state.Buttons.A == ButtonState.Released) {
            gamepad1boutonA = false;
        }
    }
}
