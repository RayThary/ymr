using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossAppear : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float speed = 20;

    private RectTransform rectTrs;
    private TextMeshProUGUI bossName;
    private TextMeshProUGUI anyKeyText;

    [SerializeField] private List<Sprite> bossSprite = new List<Sprite>();
    private Image bossImage;

    private bool stopCheck = false;
    private bool readyCheck = false;

    void Start()
    {
        GameManager.instance.SetStart(false);




        anim = GetComponent<Animator>();
        GameManager.instance.SetStart(false);
        rectTrs = transform.GetChild(1).GetComponent<RectTransform>();
        bossName = rectTrs.GetComponentInChildren<TextMeshProUGUI>();
        //bossName.text = (bossName.text == null ? "MisingEnemy" : GameManager.instance.BossName);
        bossImage = transform.GetChild(2).GetComponent<Image>();

        int bulidNum = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < 4; i++)
        {
            if (i + 2 == bulidNum)
            {
                bossImage.sprite = bossSprite[i];
            }
        }

        anyKeyText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        anyKeyText.text = string.Empty;

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            bossName.text = "Åä³¢ ±«ÇÑ";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            bossName.text = "°©¿Ê ¸¶¹ý»ç";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            bossName.text = "°©¿Ê ÁÖ¼ú»ç";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            bossName.text = "¾ÏÈæ ±â»ç";

        }

    }

    // Update is called once per frame
    void Update()
    {
        backGroundCheck();
        keyCheck();
    }

    private void backGroundCheck()
    {
        if (stopCheck)
        {
            return;
        }

        if (rectTrs.localPosition.x == -47)
        {
            anyKeyText.text = "Press Any Key";
            stopCheck = true;
            readyCheck = true;
        }
    }

    private void keyCheck()
    {
        if (readyCheck)
        {
            if (Input.anyKey == true)
            {
                readyCheck = false;
                anyKeyText.text = string.Empty;
                anim.SetTrigger("Start");
                GameManager.instance.SetStageNum();
            }
        }
    }


    //¾Ö´Ï¸ÞÀÌ¼Ç¿ë

    private void SetStartCheck()
    {
        GameManager.instance.SetStart(true);
        GameManager.instance.GetPlayer.InputKey = true;
    }

}
