using UnityEngine;
using Prime31;
using GamepadInput;
using static GamepadInput.ip_GamePad;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityWeld.Binding;


[Binding]
public class GameMenuManager3 : BaseViewModel {

    private int position;
    private GamepadState gamepadState;

    [Space(20)]
    [SerializeField]
    private String title;

    [Binding]
    public string TitlePreview {
        get => title;
    }

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

    [Space(20)]
    [SerializeField]
    private Sprite selectionSpriteV;
    [SerializeField]
    private Sprite selectionSpriteU;
    [SerializeField]
    private bool selection;

    [Space(20)]
    [SerializeField]
    private GameObject ContinuePanel;

    [Space(10)]
    [SerializeField]
    private GameObject menu2;

    [Space(10)]
    [SerializeField]
    private GameObject canvas;

    [Space(10)]
    [SerializeField]
    private GameObject fade;


    void OnEnable() {
        selection = false;
        cursor.GetComponent<SpriteRenderer>().sprite = selectionSpriteU;
    }

        // Start is called before the first frame update
        void Start() {
        canvas.SetActive(true);
        position = 1;
        this.gamepadState = new GamepadState();
        pressed = false;
        selection = false;
    }

    // Update is called once per frame
    void Update() {
        ip_GamePad.GetState(ref this.gamepadState, ip_GamePad.Index.Any);
        pressed = false;
        if(!selection) {

            if(this.position == 4) {
                if(this.gamepadState.RightPressed && pressed == false) {
                    this.position = 1;
                    pressed = true;
                }

                if(this.gamepadState.LeftPressed && pressed == false) {
                    this.position = 3;
                    pressed = true;
                }
            }
            if(this.position == 3) {
                if(this.gamepadState.RightPressed && pressed == false) {
                    this.position = 4;
                    pressed = true;
                }
                if(this.gamepadState.LeftPressed && pressed == false) {
                    this.position = 2;
                    pressed = true;
                }
            }
            if(this.position == 2) {
                if(this.gamepadState.RightPressed && pressed == false) {
                    this.position = 3;
                    pressed = true;
                }
                if(this.gamepadState.LeftPressed && pressed == false) {
                    this.position = 1;
                    pressed = true;
                }
            }
            if(this.position == 1) {
                if(this.gamepadState.RightPressed && pressed == false) {
                    this.position = 2;
                    pressed = true;
                }
                if(this.gamepadState.LeftPressed && pressed == false) {
                    this.position = 4;
                    pressed = true;
                }
            }
        }


        if(!selection && gamepadState.A) {
            cursor.GetComponent<SpriteRenderer>().sprite = selectionSpriteV;
            selection = true;
            ContinuePanel.SetActive(true);
        }
        if(selection && gamepadState.B) {
            cursor.GetComponent<SpriteRenderer>().sprite = selectionSpriteU;
            selection = false;
            ContinuePanel.SetActive(false);
        }

        if(selection) {
            if(this.gamepadState.Start) {
                SceneManager.LoadScene(0);
            }
        }

        if(this.position == 1) {
            cursor.GetComponent<Transform>().position = pos1;
            bigPreview.GetComponent<SpriteRenderer>().sprite = lvlA;
            title = "Random";
        }
        if(this.position == 2) {
            cursor.GetComponent<Transform>().position = pos2;
            bigPreview.GetComponent<SpriteRenderer>().sprite = lvl1;
            title = "Jungle";
        }
        if(this.position == 3) {
            cursor.GetComponent<Transform>().position = pos3;
            bigPreview.GetComponent<SpriteRenderer>().sprite = lvl2;
            title = "Desert";
        }
        if(this.position == 4) {
            cursor.GetComponent<Transform>().position = pos4;
            bigPreview.GetComponent<SpriteRenderer>().sprite = lvl3;
            title = "High Montain";
        }

        this.RaisePropertyChanged(nameof(this.TitlePreview));

        if(gamepadState.Back) 
            {
            canvas.SetActive(false);
            fade.SetActive(false);
            fade.SetActive(true);
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                this.menu2.SetActive(true);
                this.gameObject.SetActive(false);
            }, 0.8f));
        }

    }

}
