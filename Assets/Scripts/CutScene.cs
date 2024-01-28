using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public int screenPosition;
    public int totalScreens;
    public GameObject[] screens;
    public GameObject fadeIn;
    public GameObject fadeOut;
    private GameObject actualScreen;

    private void Start()
    {
        Instantiate(fadeIn);
        actualScreen = Instantiate(screens[screenPosition].gameObject);
        screenPosition++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            Instantiate(fadeIn);
            if (actualScreen != null)
            {
                Destroy(actualScreen);
                Instantiate(fadeOut);
            }
            actualScreen = Instantiate(screens[screenPosition].gameObject);
            
            screenPosition++;
        }
    }
}
