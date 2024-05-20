using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject backGround;
    [SerializeField]
    private GameObject soundOption;
    [SerializeField]
    private GameObject graphicOption;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            backGround.SetActive(!backGround.activeSelf);
            if(backGround.activeSelf)
            {
                GameManager.instance.GameStop();
                GameManager.instance.GetPlayer.InputKey = false;
            }
            else
            {
                GameManager.instance.GamePlay();
                GameManager.instance.GetPlayer.InputKey = true;
                soundOption.gameObject.SetActive(false);
                graphicOption.gameObject.SetActive(false);
            }
        }
    }

    public void Playing()
    {
        backGround.SetActive(false);
        GameManager.instance.GamePlay();
        GameManager.instance.GetPlayer.InputKey = true;
    }
    public void Again()
    {
        GameManager.instance.RestartGame();
        GameManager.instance.GamePlay();
        GameManager.instance.GetPlayer.InputKey = true;
    }
    public void MainMenu()
    {
        GameManager.instance.MainMenitScenesLoad(); 
        GameManager.instance.GamePlay();
        GameManager.instance.GetPlayer.InputKey = true;
    }
    public void Sound()
    {
        backGround.gameObject.SetActive(false);
        soundOption.gameObject.SetActive(true);
    }
    public void Graphic()
    {
        backGround.gameObject.SetActive(false);
        graphicOption.gameObject.SetActive(true);
    }
    public void Exit()
    {
        GameManager.instance.ExitGame();
    }
}
