using UnityEngine;
using Prime31;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityWeld.Binding;


[Binding]
public class GameMenuManager3 : BaseViewModel {

    static public int selectedLevel;

    private int position;
    private GamePadState gamepadState;

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

    [SerializeField]
    private AudioSource menuSelect;
    [SerializeField]
    private AudioSource startValidate;
    [SerializeField]
    private AudioSource selectLVL;
    [SerializeField]
    private AudioSource menuBack;

    private bool dpadUp;

    private bool dpadDown;

    private bool dpadR;

    private bool dpadL;


    void OnEnable() {
        selection = false;
        cursor.GetComponent<SpriteRenderer>().sprite = selectionSpriteU;
    }

        // Start is called before the first frame update
        void Start() {
        canvas.SetActive(true);
        position = 1;
        pressed = false;
        selection = false;
    }

    // Update is called once per frame
    void Update() {

        PlayerIndex index = (PlayerIndex.One);

            ///////////////////////////////////////////////////////////////////////////////

            //old pad script

            if(this.gamepadState.DPad.Up == ButtonState.Released) {
                dpadUp = false;
            }

            if(this.gamepadState.DPad.Down == ButtonState.Released) {
                dpadDown = false;
            }

            if(this.gamepadState.DPad.Right == ButtonState.Released) {
                dpadR = false;
            }

            if(this.gamepadState.DPad.Left == ButtonState.Released) {
                dpadL = false;
            }


            ///////////////////////////////////////////////////////////////////////////////



            this.gamepadState = GamePad.GetState(index);
            pressed = false;
            if(!selection) {

                if(this.position == 4) {
                    if(this.gamepadState.DPad.Right == ButtonState.Pressed && dpadR == false) {
                        this.position = 1;
                        dpadR = true;
                    }

                    if(this.gamepadState.DPad.Left == ButtonState.Pressed && dpadL == false) {
                        this.position = 3;
                        dpadL = true;
                    }
                }
                if(this.position == 3) {
                    if(this.gamepadState.DPad.Right == ButtonState.Pressed && dpadR == false) {
                        this.position = 4;
                        dpadR = true;
                    }
                    if(this.gamepadState.DPad.Left == ButtonState.Pressed && dpadL == false) {
                        this.position = 2;
                        dpadL = true;
                    }
                }
                if(this.position == 2) {
                    if(this.gamepadState.DPad.Right == ButtonState.Pressed && dpadR == false) {
                        this.position = 3;
                        dpadR = true;
                    }
                    if(this.gamepadState.DPad.Left == ButtonState.Pressed && dpadL == false) {
                        this.position = 1;
                        dpadL = true;
                    }
                }
                if(this.position == 1) {
                    if(this.gamepadState.DPad.Right == ButtonState.Pressed && dpadR == false) {
                        this.position = 2;
                        dpadR = true;
                    }
                    if(this.gamepadState.DPad.Left == ButtonState.Pressed && dpadL == false) {
                        this.position = 4;
                        dpadL = true;
                    }
                }
            }


            if(!selection && this.gamepadState.Buttons.A == ButtonState.Pressed) {
                cursor.GetComponent<SpriteRenderer>().sprite = selectionSpriteV;
                selection = true;
                ContinuePanel.SetActive(true);
            }
            if(selection && this.gamepadState.Buttons.B == ButtonState.Pressed) {
                cursor.GetComponent<SpriteRenderer>().sprite = selectionSpriteU;
                selection = false;
                ContinuePanel.SetActive(false);
            }

            if(selection) {
                if(this.gamepadState.Buttons.Start == ButtonState.Pressed) {
                    if(this.position > 1) {
                        selectedLevel = this.position - 1;
                    } else {
                        selectedLevel = UnityEngine.Random.Range(1, 3);
                    }
                    SceneManager.LoadScene(1);
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

            if(this.gamepadState.Buttons.Back == ButtonState.Pressed) {
                menuBack.Play();
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
