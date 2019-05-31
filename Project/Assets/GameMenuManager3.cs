using UnityEngine;
using Prime31;
using GamepadInput;
using static GamepadInput.ip_GamePad;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameMenuManager3 : MonoBehaviour
{
    private int position;
    private GamepadState gamepadState;

    [Space(20)]
    [SerializeField]
    private Vector3 pos1;

    [SerializeField]
    private Vector3 pos2;

    [SerializeField]
    private Vector3 pos3;

    [SerializeField]
    private Vector3 pos4;

    [Space(20)]
    [SerializeField]
    private GameObject cursor;

    [Space(20)]
    [SerializeField]
    private GameObject bigPreview;
    [Space(20)]
    [SerializeField]
    private Sprite lvlA;
    [SerializeField]
    private Sprite lvl1;
    [SerializeField]
    private Sprite lvl2;
    [SerializeField]
    private Sprite lvl3;

    [Space(20)]
    [SerializeField]
    private bool pressed;
    // Start is called before the first frame update
    void Start()
    {
        position = 1;
        this.gamepadState = new GamepadState();
        pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        ip_GamePad.GetState(ref this.gamepadState, ip_GamePad.Index.Any);
        pressed = false;

        if(this.position == 4) 
            {
            if(this.gamepadState.RightPressed && pressed == false) 
            {
                this.position = 1;
                pressed = true;
            }
            
            if(this.gamepadState.LeftPressed && pressed == false)
                {
                    this.position = 3;
                    pressed = true;
                }
            }
        if(this.position == 3)
        {
            if(this.gamepadState.RightPressed && pressed == false)
            {
                this.position = 4;
                pressed = true;
            }
            if(this.gamepadState.LeftPressed && pressed == false) 
            {
                this.position = 2;
                pressed = true;
            }
        }
        if(this.position == 2) 
        {
            if(this.gamepadState.RightPressed && pressed == false) 
            {
                this.position = 3;
                pressed = true;
            }
            if(this.gamepadState.LeftPressed && pressed == false) 
            {
                this.position = 1;
                pressed = true;
            }
        }
        if(this.position == 1) 
        {
            if(this.gamepadState.RightPressed && pressed == false) 
            {
                this.position = 2;
                pressed = true;
            }
            if(this.gamepadState.LeftPressed && pressed == false) 
            {
                this.position = 4;
                pressed = true;
            }
        }

        if(this.position == 1) {
            cursor.GetComponent<Transform>().position = pos1;
            bigPreview.GetComponent<SpriteRenderer>().sprite = lvlA;
        }
        if(this.position == 2) {
            cursor.GetComponent<Transform>().position = pos2;
            bigPreview.GetComponent<SpriteRenderer>().sprite = lvl1;

        }
        if(this.position == 3) {
            cursor.GetComponent<Transform>().position = pos3;
            bigPreview.GetComponent<SpriteRenderer>().sprite = lvl2;

        }
        if(this.position == 4) {
            cursor.GetComponent<Transform>().position = pos4;
            bigPreview.GetComponent<SpriteRenderer>().sprite = lvl3;

        }
    }
}
