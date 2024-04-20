using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{
    //상속받은 자식은 부모에서 찾아주면 전부찾을수있다.
    List<testpa> listpa= new List<testpa>();
    void Start()
    {
        listpa.AddRange(GetComponentsInChildren<testpa>());

        int count = listpa.Count;
        for (int i = 0; i < count; i++)
        {
            testpa p = listpa[i];
            p.show();
        }
    }

    
    void Update()
    {
        
    }
}
