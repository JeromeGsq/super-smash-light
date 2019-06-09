using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class gamepadtesterscript : MonoBehaviour
{
    public bool gamepad1boutonA;
    public bool gamepad2boutonA;
    public bool gamepad3boutonA;
    public bool gamepad4boutonA;

    public PlayerIndex controllerNumber1 = PlayerIndex.One;
    public PlayerIndex controllerNumber2 = PlayerIndex.Two;
    public PlayerIndex controllerNumber3 = PlayerIndex.Three;
    public PlayerIndex controllerNumber4 = PlayerIndex.Four;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GamePadState state1 = GamePad.GetState(controllerNumber1);
        GamePadState state2 = GamePad.GetState(controllerNumber2);
        GamePadState state3 = GamePad.GetState(controllerNumber3);
        GamePadState state4 = GamePad.GetState(controllerNumber4);

        if(state1.Buttons.A == ButtonState.Pressed) {
            gamepad1boutonA = true;
        }
        if(state2.Buttons.A == ButtonState.Pressed) {
            gamepad2boutonA = true;
        }
        if(state3.Buttons.A == ButtonState.Pressed) {
            gamepad3boutonA = true;
        }
        if(state4.Buttons.A == ButtonState.Pressed) {
            gamepad4boutonA = true;
        }


        if(state1.Buttons.A == ButtonState.Released) {
            gamepad1boutonA = false;
        }
        if(state2.Buttons.A == ButtonState.Released) {
            gamepad2boutonA = false;
        }
        if(state3.Buttons.A == ButtonState.Released) {
            gamepad3boutonA = false;
        }
        if(state4.Buttons.A == ButtonState.Released) {
            gamepad4boutonA = false;
        }
    }
}
