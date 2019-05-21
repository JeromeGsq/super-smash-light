using UnityEngine;
using Prime31;
using GamepadInput;
using static GamepadInput.ip_GamePad;
using UnityEngine.SceneManagement;
using System;

public class GameMenuManager : MonoBehaviour {
    public int positions;
    public GameObject fond;
    public GameObject illu;
    public Sprite fondPartie;
    public Sprite fondOption;

    public Sprite illuPartie;
    public Sprite illuOption;

    public GameObject BoutonPartie;
    public GameObject BoutonOption;

    public GameObject paternHolder;
    public Sprite paternPartie;
    public Sprite paternOption;

    public GameObject selecteur;

    private GamepadState gamepadState;


    // Start is called before the first frame update
    void Awake() {
        positions = 1;
        this.gamepadState = new GamepadState();
    }

    // Update is called once per frame


    void Update() {


    ip_GamePad.GetState(ref this.gamepadState, ip_GamePad.Index.Any);
        if(positions == 1) {
            selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -2.23f, -0.61f);
            fond.GetComponent<SpriteRenderer>().sprite = fondPartie;
            illu.GetComponent<SpriteRenderer>().sprite = illuPartie;
            GameObject.Find("patern1").GetComponent<SpriteRenderer>().sprite = paternPartie;
            GameObject.Find("patern2").GetComponent<SpriteRenderer>().sprite = paternPartie;
            GameObject.Find("patern3").GetComponent<SpriteRenderer>().sprite = paternPartie;
            GameObject.Find("patern4").GetComponent<SpriteRenderer>().sprite = paternPartie;
            if(this.gamepadState.APressed) {
                BoutonPartie.GetComponent<Animator>().Play(Animator.StringToHash("PartieRapideMenu"));
            }
            
        } else {
            selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -5.43f, -0.61f);
            fond.GetComponent<SpriteRenderer>().sprite = fondOption;
            illu.GetComponent<SpriteRenderer>().sprite = illuOption;
            GameObject.Find("patern1").GetComponent<SpriteRenderer>().sprite = paternOption;
            GameObject.Find("patern2").GetComponent<SpriteRenderer>().sprite = paternOption;
            GameObject.Find("patern3").GetComponent<SpriteRenderer>().sprite = paternOption;
            GameObject.Find("patern4").GetComponent<SpriteRenderer>().sprite = paternOption;
            if(this.gamepadState.APressed) {
                BoutonOption.GetComponent<Animator>().Play(Animator.StringToHash("OptionMenu"));
            }
        }

        if(positions == 1 && this.gamepadState.Down) {
            positions = 2;

        }
        if(positions == 2 && this.gamepadState.Up) {
            positions = 1;

        }
    }
    }
