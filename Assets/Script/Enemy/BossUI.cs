using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUI : MonoBehaviour
{
    public static BossUI Instance;

    Stat statBoss = null;
    TMP_Text textBossName;
    GameObject objUiHp;
    Image ImageHpMask;

    public Stat StatBoss
    {
        set 
        { 
            statBoss = value;
            objUiHp.SetActive(true);
            textBossName.text = statBoss.BossName == string.Empty ? "Unknown Enemy" : statBoss.BossName;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        objUiHp = transform.Find("BackGround").gameObject;

        textBossName = objUiHp.transform.Find("BossName").GetComponent<TMP_Text>();
        ImageHpMask = objUiHp.transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        if (statBoss != null)
        {
            ImageHpMask.fillAmount = statBoss.HP / statBoss.MAXHP;
        }
        else
        {
            objUiHp.SetActive(false);
        }
    }
}
