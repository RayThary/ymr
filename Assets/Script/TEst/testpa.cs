using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testpa : MonoBehaviour
{
    protected float Hp;


    protected virtual void Start()
    {
        Debug.Log("�θ��� Start�Լ�");
    }

    
    protected virtual void Update()
    {
        Debug.Log("�θ��� update�Լ�");
    }

    public virtual void show()
    {
        Debug.Log("�θ��� Show�Լ�");
    }
}
