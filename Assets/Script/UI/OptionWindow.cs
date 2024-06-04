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
    [SerializeField]
    private GameObject howtoOption;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(soundOption.activeSelf)
            {
                soundOption.SetActive(false);
            }
            else if(howtoOption.activeSelf)
            {
                howtoOption.SetActive(false);
            }
            else if(graphicOption.activeSelf)
            {
                graphicOption.SetActive(false);
            }
            else if (backGround.activeSelf)
            {
                backGround.SetActive(false);
            }
            else
            {
                backGround.SetActive(true);
            }
            SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
            if (backGround.activeSelf)
            {
                GameManager.instance.GameStop();
                GameManager.instance.GetPlayer.InputKey = false;
            }
            else
            {
                GameManager.instance.GamePlay();
                GameManager.instance.GetPlayer.InputKey = true;
            }
        }
    }

    public void Playing()
    {
        backGround.SetActive(false);
        GameManager.instance.GamePlay();
        GameManager.instance.GetPlayer.InputKey = true;
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }
    public void Again()
    {
        GameManager.instance.RestartGame();
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }
    public void MainMenu()
    {
        GameManager.instance.MainMenitScenesLoad(); 
        GameManager.instance.GamePlay();
        GameManager.instance.GetPlayer.InputKey = true;
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }
    public void Sound()
    {
        soundOption.gameObject.SetActive(true);
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }
    public void SoundClose()
    {
        soundOption.gameObject.SetActive(false);
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }
    public void Graphic()
    {
        graphicOption.gameObject.SetActive(true);
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }
    public void GraphicClose()
    {
        graphicOption.gameObject.SetActive(false);
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }
    public void Howto()
    {
        howtoOption.gameObject.SetActive(true);
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }
    public void HowtoClose()
    {
        howtoOption.gameObject.SetActive(false);
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }
    public void Exit()
    {
        GameManager.instance.ExitGame();
    }
}
