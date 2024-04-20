using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testpa : MonoBehaviour
{
    protected float Hp;


    protected virtual void Start()
    {
        Debug.Log("부모의 Start함수");
    }

    
    protected virtual void Update()
    {
        Debug.Log("부모의 update함수");
    }

    public virtual void show()
    {
        Debug.Log("부모의 Show함수");
    }
}
