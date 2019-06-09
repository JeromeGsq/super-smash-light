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

    public GameObject paternHolder;
    public Sprite paternPartie;
    public Sprite paternOption;

    public GameObject selecteur;

    private GamePadState gamepadState;

    public GameObject MenuPrincipal;
    public GameObject SelectPlayerMenu;
    public GameObject AnimatedBandeau;


    // Start is called before the first frame update
    void Awake() {
        AnimatedBandeau.SetActive(false);
        positions = 1;
    }

    // Update is called once per frame
    private void Start() {
        AnimatedBandeau.SetActive(false);

    }

    void Update() {

        for(int i = 0; i < 4; ++i) {
            PlayerIndex index = (PlayerIndex)i;


            this.gamepadState = GamePad.GetState(index);
            if(positions == 1) {
                selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -2.23f, selecteur.GetComponent<Transform>().position.z);
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
                    }, 0.3f));

                    StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                        SelectPlayerMenu.SetActive(true);
                        MenuPrincipal.SetActive(false);
                    }, 0.7f));
                }

            } else {
                selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -5.43f, selecteur.GetComponent<Transform>().position.z);
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
            }

            if(positions == 1 && this.gamepadState.DPad.Down == ButtonState.Pressed) {
                positions = 2;

            }
            if(positions == 2 && this.gamepadState.DPad.Up == ButtonState.Pressed) {
                positions = 1;

            }
        }
    }
    }
