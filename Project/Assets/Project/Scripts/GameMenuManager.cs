using UnityEngine;
using Prime31;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using System;

public class GameMenuManager : MonoBehaviour {
    public int positions;
    public GameObject fond;
    public GameObject illu;
    public GameObject illu1;
    public GameObject illu2;

    public Sprite fondPartie;
    public Sprite fondOption;

    public Sprite illuPartie;
    public Sprite illuPartie1;
    public Sprite illuPartie2;

    public Sprite illuOption;
    public Sprite illuOption1;

    public GameObject BoutonPartie;
    public GameObject BoutonOption;
    public GameObject BoutonQuitter;

    public GameObject paternHolder;
    public Sprite paternPartie;
    public Sprite paternOption;

    public GameObject selecteur;

    private GamePadState gamepadState;

    public GameObject MenuPrincipal;
    public GameObject SelectPlayerMenu;
    public GameObject AnimatedBandeau;

    public bool pressedUp;
    public bool pressedDown;
    [SerializeField]
    public bool pressedA;
    [SerializeField]
    public GameObject returnWindows;


    // Start is called before the first frame update
    void Awake() {
        AnimatedBandeau.SetActive(false);
        positions = 1;
    }

    // Update is called once per frame
    private void Start() {
        AnimatedBandeau.SetActive(false);
        pressedA = true;
        

    }

    void Update() {

       // for(int i = 0; i < 4; ++i) {
            PlayerIndex index = (PlayerIndex.One);


            this.gamepadState = GamePad.GetState(index);
            if(positions == 1) {
                selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -1.25f, selecteur.GetComponent<Transform>().position.z);
                fond.GetComponent<SpriteRenderer>().sprite = fondPartie;
                illu.GetComponent<SpriteRenderer>().sprite = illuPartie;
                illu1.GetComponent<SpriteRenderer>().sprite = illuPartie1;
                illu2.GetComponent<SpriteRenderer>().sprite = illuPartie2;
                illu2.GetComponent<Transform>().localPosition = new Vector3(-0.34f, -0.96f, -0.21f);
                GameObject.Find("patern1").GetComponent<SpriteRenderer>().sprite = paternPartie;
                GameObject.Find("patern2").GetComponent<SpriteRenderer>().sprite = paternPartie;
                GameObject.Find("patern3").GetComponent<SpriteRenderer>().sprite = paternPartie;
                GameObject.Find("patern4").GetComponent<SpriteRenderer>().sprite = paternPartie;
                if(this.gamepadState.Buttons.A == ButtonState.Pressed) {
                    BoutonPartie.GetComponent<Animator>().Play(Animator.StringToHash("PartieRapideMenu"));
                    StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                        AnimatedBandeau.SetActive(false);
                        AnimatedBandeau.SetActive(true);
                    }, 0.1f));

                    StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                        SelectPlayerMenu.SetActive(true);
                        MenuPrincipal.SetActive(false);
                    }, 0.7f));
                }

            } else if(positions == 2) {
                selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -4.5f, selecteur.GetComponent<Transform>().position.z);
                fond.GetComponent<SpriteRenderer>().sprite = fondOption;
                illu.GetComponent<SpriteRenderer>().sprite = illuOption;
                illu1.GetComponent<SpriteRenderer>().sprite = null;
                illu2.GetComponent<SpriteRenderer>().sprite = illuOption1;
                illu2.GetComponent<Transform>().localPosition = new Vector3(-2.49f, 1.23f, -0.21f);
                GameObject.Find("patern1").GetComponent<SpriteRenderer>().sprite = paternOption;
                GameObject.Find("patern2").GetComponent<SpriteRenderer>().sprite = paternOption;
                GameObject.Find("patern3").GetComponent<SpriteRenderer>().sprite = paternOption;
                GameObject.Find("patern4").GetComponent<SpriteRenderer>().sprite = paternOption;
                if(this.gamepadState.Buttons.A == ButtonState.Pressed) {
                    BoutonOption.GetComponent<Animator>().Play(Animator.StringToHash("OptionMenu"));
                }
            } else {
                selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -7.75f, selecteur.GetComponent<Transform>().position.z);
                fond.GetComponent<SpriteRenderer>().sprite = fondOption;
                illu.GetComponent<SpriteRenderer>().sprite = illuOption;
                illu1.GetComponent<SpriteRenderer>().sprite = null;
                illu2.GetComponent<SpriteRenderer>().sprite = illuOption1;
                illu2.GetComponent<Transform>().localPosition = new Vector3(-2.49f, 1.23f, -0.21f);
                GameObject.Find("patern1").GetComponent<SpriteRenderer>().sprite = paternOption;
                GameObject.Find("patern2").GetComponent<SpriteRenderer>().sprite = paternOption;
                GameObject.Find("patern3").GetComponent<SpriteRenderer>().sprite = paternOption;
                GameObject.Find("patern4").GetComponent<SpriteRenderer>().sprite = paternOption;
            
            
            //Quitter le jeu
            if (gamepadState.Buttons.A == ButtonState.Pressed && pressedDown == false)
            {
                pressedDown = true;
                BoutonQuitter.GetComponent<Animator>().Play(Animator.StringToHash("OptionMenu"));

                StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                    MenuPrincipal.SetActive(false);
                    returnWindows.SetActive(true);
                }, 0.7f));

                Debug.Log("switch to BackToWindows script");
            }
            //if (this.gamepadState.Buttons.A == ButtonState.Pressed) {
            //        BoutonQuitter.GetComponent<Animator>().Play(Animator.StringToHash("OptionMenu"));
            //         Application.Quit();
            //    Debug.Log("Quitter le jeu");
            //}
            }

            if(positions == 1 && this.gamepadState.DPad.Down == ButtonState.Pressed && pressedDown == false) {
                pressedDown = true;
                positions = 2;
                
            }
            if(positions == 2 && this.gamepadState.DPad.Up == ButtonState.Pressed && pressedUp == false) {
                pressedUp = true;
                positions = 1;

            }
            if(positions == 2 && this.gamepadState.DPad.Down == ButtonState.Pressed && pressedDown == false) {
                pressedDown = true;
                positions = 3;

            }
            if(positions == 3 && this.gamepadState.DPad.Up == ButtonState.Pressed && pressedUp == false) {
                pressedUp = true;
                positions = 2;

            }
            if(pressedUp == true && this.gamepadState.DPad.Up == ButtonState.Released) {
                pressedUp = false;
            }
            if(pressedDown == true && this.gamepadState.DPad.Down == ButtonState.Released) {
                pressedDown = false;
            }
        //}
    }
    }
