using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{
    //��ӹ��� �ڽ��� �θ𿡼� ã���ָ� ����ã�����ִ�.
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
