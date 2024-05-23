using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;



public class TextSelect : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    private TextMeshProUGUI m_text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_text.fontStyle = FontStyles.Italic;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_text.fontStyle = FontStyles.Normal;
    }


    private void Start()
    {
        m_text = GetComponentInChildren<TextMeshProUGUI>();   
    }


}
