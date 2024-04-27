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
        bossName.text = (bossName.text == null ? "MisingEnemy" : GameManager.instance.BossName);
        bossImage = transform.GetChild(2).GetComponent<Image>();

        int bulidNum = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < 4; i++)
        {
            if (i == bulidNum - 1)
            {
                bossImage.sprite = bossSprite[i];
            }
        }

        
        

        anyKeyText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        anyKeyText.text = string.Empty;
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

        if (rectTrs.localPosition.x ==  -47)
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
            if(Input.anyKey==true)
            {
                readyCheck = false;
                anyKeyText.text = string.Empty;
                anim.SetTrigger("Start");
                GameManager.instance.SetStageNum();
            }
        }
    }


    //애니메이션용

    private void SetStartCheck()
    {
        GameManager.instance.SetStart(true);
    }
   
}
