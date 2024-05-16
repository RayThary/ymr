using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    public Image hp;
    private Stat _stat;
    public Stat Stat { get { return _stat; } set { _stat = value; } }

    // Start is called before the first frame update
    void Start()
    {
        _stat = GameManager.instance.GetPlayer.STAT;
    }

    // Update is called once per frame
    void Update()
    {
        hp.fillAmount = _stat.HP/_stat.MAXHP;
    }
}
