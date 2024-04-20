using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone_LineRenderer : MonoBehaviour
{
    private LineRenderer line;
    Gradient gr = new Gradient();
    private GradientAlphaKey[] rKey = new GradientAlphaKey[2];
    private GradientColorKey[] cKey = new GradientColorKey[2];
    [SerializeField] private float time = 2;

    private bool alphaCheck;

    [SerializeField] private float maxAlpha = 0.8f;
    [SerializeField] private float minAlpha = 0.5f;

    private float alpha = 0.9f;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        cKey[0] = new GradientColorKey(Color.red, 0);
        cKey[1] = new GradientColorKey(Color.red, 1);
    }

   void Update()
    {
        alphaControl();
        objCheck();
    }
    private void alphaControl()
    {
        rKey[0] = new GradientAlphaKey(alpha, 0);
        rKey[1] = new GradientAlphaKey(alpha, 1);
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

        gr.SetKeys(cKey, rKey);

        line.colorGradient = gr;
    }

    private void objCheck()
    {
        if (line.enabled == true)
        {
            StartCoroutine(offObj());
        }
    }
    IEnumerator offObj()
    {
        yield return new WaitForSeconds(time);
        line.enabled = false;
    }

    public float getTime()
    {
        return time;
    }
    public void SetTime(float _value)
    {
        time = _value;
    }
}
