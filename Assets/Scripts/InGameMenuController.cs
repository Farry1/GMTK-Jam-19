﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuController : MonoBehaviour
{  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    public void OpenInstructions()
    {
        UIController.Instance.inGameMenu.SetActive(false);
        UIController.Instance.instructions.SetActive(true);
    }

}