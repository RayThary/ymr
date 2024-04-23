using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{

    [SerializeField] private float time = 2;

    private SpriteRenderer spr;
    private Color color;
    private bool alphaCheck;

    [SerializeField] private float maxAlpha = 0.8f;
    [SerializeField] private float minAlpha = 0.5f;

    private float alpha = 0.9f;

 
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        color = spr.color;
    }


    protected virtual void Update()
    {
        alphaControl();
        objCheck();
    }

    private void alphaControl()
    {
        if (alpha <= minAlpha)
        {
            alphaCheck = true;
        }
        else if (alpha >= maxAlpha)
        {
            alphaCheck = false;
        }

        if (alphaCheck)
        {
            alpha += Time.deltaTime * 0.5f;
        }
        else
        {
            alpha -= Time.deltaTime * 0.5f;
        }

        color.a = alpha;
        spr.color = color;
    }

    private void objCheck()
    {
        if (spr.enabled == true)
        {
            StartCoroutine(offObj());
        }
    }
    IEnumerator offObj()
    {
        yield return new WaitForSeconds(time);
        spr.enabled = false;
    }

    public float getTime()
    {
        return time;
    }
    public void SetTime(float _value)
    {
        time = _value;
    }

    public void setSprite(bool _value)
    {
        spr.enabled = _value;
    }
}
