using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButterflyBomp : MonoBehaviour
{
    private SpriteRenderer spr;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        alphaCheck();
    }

    private void alphaCheck()
    {
        Color color = spr.color;
        if (color.a != 0.0f)
        {
            color.a -= Time.deltaTime;
            if (color.a < 0.0f)
            {
                color.a = 0.0f;
                PoolingManager.Instance.RemovePoolingObject(transform.parent.gameObject);
                color.a = 1f;
            }
        }
        spr.color = color;
    }
}
