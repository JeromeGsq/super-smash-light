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

    [Space(20)]
    [SerializeField]
    private Vector3 pos2;

    [Space(20)]
    [SerializeField]
    private Vector3 pos3;

    [Space(20)]
    [SerializeField]
    private Vector3 pos4;

    [Space(20)]
    [SerializeField]
    private GameObject cursor;

    [Space(20)]
    [SerializeField]
    private GameObject bigPreview;

    // Start is called before the first frame update
    void Start()
    {
        this.gamepadState = new GamepadState();
    }

    // Update is called once per frame
    void Update()
    {
        if(position == 1) 
            {
            if(gamepadState.RightPressed) 
            {
                position = 2;
            }
        }
        if(position == 2)
        {
            if(gamepadState.RightPressed)
            {
                position = 3;
            }
        }
        if(position == 3) 
        {
            if(gamepadState.RightPressed) 
            {
                position = 4;
            }
        }
        if(position == 4) 
        {
            if(gamepadState.RightPressed) 
            {
                position = 1;
            }
        }
    }
}
