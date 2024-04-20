using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiText : MonoBehaviour
{
    private TextMeshProUGUI m_text;
    private float timer = 0;
    void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            m_text.color = new Color(Random.Range(0.8f, 1), Random.Range(0.8f, 1), Random.Range(0.8f, 1), 1f);
            timer = 0;
        }
    }
}
