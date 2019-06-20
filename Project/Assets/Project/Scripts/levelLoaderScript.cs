using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelLoaderScript : MonoBehaviour
{
    [Space(10)]
    [SerializeField]
    private GameObject lv1;

    [Space(10)]
    [SerializeField]
    private GameObject lv2;

    [Space(10)]
    [SerializeField]
    private GameObject lv3;

    // Start is called before the first frame update
    void Start()
    {
        if(GameMenuManager3.selectedLevel == 1)
        {
            Instantiate(lv1, null);
        }
        if(GameMenuManager3.selectedLevel == 2) 
        {
            Instantiate(lv2, null);
        }
        if(GameMenuManager3.selectedLevel == 3) 
        {
            Instantiate(lv3, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
