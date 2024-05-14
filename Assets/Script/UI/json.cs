using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Data 
{
    public GameObject optionWindow;

    public float masterVolume;
    public float SFXVolume;
    public float backGroundVolume;

}


public class json : MonoBehaviour
{
    Data OptionWindow = new Data();

    void Start()
    {
        OptionWindow.optionWindow = gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
