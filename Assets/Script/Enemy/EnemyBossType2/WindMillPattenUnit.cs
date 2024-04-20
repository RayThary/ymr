using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMillPattenUnit : MonoBehaviour
{
    private Transform lenth;
    private Transform width;

    [SerializeField] private Unit boss;
    public Unit Boss { set => boss = value; }
    void Start()
    {
        lenth = GetComponentInChildren<Transform>().Find("WindMillPattenLenght");
        width = GetComponentInChildren<Transform>().Find("WindMillPattenWidth");
        lenth.GetComponent<WindMillPatten>().Boss = boss;
        width.GetComponent<WindMillPatten>().Boss = boss;
    }

}
