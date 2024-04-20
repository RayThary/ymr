using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class HaxagonPatten : MonoBehaviour
{

    private LineRenderer lineRen;
    private List<Transform> startTrs= new List<Transform>(); //각각 선들의 꼭지점들

    [SerializeField] private float speed = 4;
    
    void Start()
    {
        lineRen = GetComponent<LineRenderer>();
        float startY = Random.Range(0, 7);

        float angleY = 0;

        if (startY == 0)
        {
            angleY = 0;
        }
        else if(startY == 1) 
        {
            angleY = 60;
        }
        else if (startY == 2)
        {
            angleY = 120;
        }
        else if (startY == 3)
        {
            angleY = 180;
        }
        else if (startY == 4)
        {
            angleY = 240;
        }
        else if (startY == 5)
        {
            angleY = 300;
        }
        else
        {
            angleY = 360;
        }



        
        for (int i = 0; i < 6; i++)
        {
            startTrs.Add(transform.GetChild(i));
            startTrs[i].transform.rotation = Quaternion.Euler(0, angleY, 0);
            angleY += 60;

        }

        Invoke("removeObj", 15);


    }

    // Update is called once per frame
    void Update()
    {
        lineMove();
        hitCheck();
    }

    private void lineMove()
    {
        for (int i = 0; i < 6; i++)
        {
            startTrs[i].position += startTrs[i].forward * Time.deltaTime * speed;
           
            lineRen.SetPosition(i, startTrs[i].position);
        }
    }

    private void hitCheck()
    {
        Player player = GameManager.instance.GetPlayer;
        if (Physics.Linecast(startTrs[0].position, startTrs[1].position, LayerMask.GetMask("Player")))
        {
            player.Hit(null, 1);
        }
        if (Physics.Linecast(startTrs[1].position, startTrs[2].position, LayerMask.GetMask("Player")))
        {
            player.Hit(null, 1);
        }
        if (Physics.Linecast(startTrs[2].position, startTrs[3].position, LayerMask.GetMask("Player")))
        {
            player.Hit(null, 1);
        }
        if (Physics.Linecast(startTrs[3].position, startTrs[4].position, LayerMask.GetMask("Player")))
        {
            player.Hit(null, 1);
        }
        if (Physics.Linecast(startTrs[4].position, startTrs[5].position, LayerMask.GetMask("Player")))
        {
            player.Hit(null, 1);
        }
        
    }

    private void removeObj()
    {
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
}
