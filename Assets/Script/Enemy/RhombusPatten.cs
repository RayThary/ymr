using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhombusPatten : MonoBehaviour
{
    private LineRenderer lineRen;
    private List<Transform> startTrs = new List<Transform>(); //각각 선들의 꼭지점들

    [SerializeField] private float speed = 4;
    void Start()
    {
        lineRen = GetComponent<LineRenderer>();
        float startY = Random.Range(0, 5);

        float angleY = 0;

        if (startY == 0)
        {
            angleY = 0;
        }
        else if (startY == 1)
        {
            angleY = 90;
        }
        else if (startY == 2)
        {
            angleY = 180;
        }
        else if (startY == 3)
        {
            angleY = 270;
        }
        else
        {
            angleY = 360;
        }




        for (int i = 0; i < 4; i++)
        {
            startTrs.Add(transform.GetChild(i));
            startTrs[i].transform.rotation = Quaternion.Euler(0, angleY, 0);
            angleY += 90;

        }

    }

    // Update is called once per frame
    void Update()
    {
        lineMove();
        hitCheck();
    }

    private void lineMove()
    {
        for (int i = 0; i < 4; i++)
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
       
    }
}
