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
        position = 1;
        this.gamepadState = new GamepadState();
    }

    // Update is called once per frame
    void Update()
    {
        ip_GamePad.GetState(ref this.gamepadState, ip_GamePad.Index.Any);


        if(this.position == 4) 
            {
            if(this.gamepadState.Right) 
            {
                this.position = 1;
            }
        }
        if(this.position == 3)
        {
            if(this.gamepadState.Right)
            {
                this.position = 4;
            }
        }
        if(this.position == 2) 
        {
            if(this.gamepadState.Right) 
            {
                this.position = 3;
            }
        }
        if(this.position == 1) 
        {
            if(this.gamepadState.Right) 
            {
                this.position = 2;
            }
        }

        if(this.position == 1) {
            cursor.GetComponent<Transform>().position = pos1;
        }
        if(this.position == 2) {
            cursor.GetComponent<Transform>().position = pos2;
        }
        if(this.position == 3) {
            cursor.GetComponent<Transform>().position = pos3;
        }
        if(this.position == 4) {
            cursor.GetComponent<Transform>().position = pos4;
        }
    }
}
