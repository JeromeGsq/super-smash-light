using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class canvasClignotement : MonoBehaviour
{
    public Color bkgColor;
    public Color black;
    public Color white;
    public float timer;
    public int theColor;

    // Start is called before the first frame update
    void Start()
    {
        theColor = 0;
        bkgColor = gameObject.GetComponent<Image>().color;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(theColor == 0 && timer > 0.15f) {
            theColor = 1;
            gameObject.GetComponent<Image>().color = white;
            timer = 0;
        }
        if(theColor == 1 && timer > 0.15f) {
            theColor = 0;
            gameObject.GetComponent<Image>().color = black;
            timer = 0;
        }
    }
}
