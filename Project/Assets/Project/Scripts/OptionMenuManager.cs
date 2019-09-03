using UnityEngine;
using Prime31;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class OptionMenuManager : MonoBehaviour
{
    static public int Position = 0;

     [SerializeField]
    private GameObject languageEn;

    [SerializeField]
    public GameObject easy;
    [SerializeField]
    public GameObject medium;
    [SerializeField]
    public GameObject hard;
    [SerializeField]
    public bool pressedUp;
    [SerializeField]
    public bool pressedDown;
    [SerializeField]
    private bool pressedBack = true;
    [SerializeField]
    public bool pressedA = true;
    [SerializeField]
    public GameObject selecteur;


    // Start is called before the first frame update
    void Start()
    {
        easy.SetActive(true);
        languageEn.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
            if (Position == 1)
            {
                selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -4.5f, selecteur.GetComponent<Transform>().position.z);
            }

            else if (Position == 2)
            {
                selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -4.5f, selecteur.GetComponent<Transform>().position.z);
            }

          else if (Position == 3)
            {
                selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -4.5f, selecteur.GetComponent<Transform>().position.z);
            }

            else if (Position == 4)
            {
                selecteur.GetComponent<Transform>().position = new Vector3(6.18f, -4.5f, selecteur.GetComponent<Transform>().position.z);
            }
    }
}