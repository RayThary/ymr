using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteAlphaControl : MonoBehaviour
{
    public bool isHit = false; // 체크시 피격된걸로 반짝거림 근데 instance로 나중에따로 private로 해주거나 아니면 추가해서사용할필요있음

    [SerializeField] private float ReturnTime = 0.5f;//투명도 원상복귀시간
    [SerializeField] private float Transparency = 0.5f;//투명도 비율조정해주셈

    private Transform[] childList;// 모든자식들모음
    private List<SpriteRenderer> m_spr = new List<SpriteRenderer>();//스프라이랜더러있는것들모음

    private List<Color> m_alphaNormal = new List<Color>();//기본색깔
    private List<Color> m_alphaTranslucent = new List<Color>();//반투명색깔
    private Color m_isTranslucent;

    private int isSprite;// 스프라이트개수
    void Start()
    {


        childList = transform.GetComponentsInChildren<Transform>();
        findSprite();
        
    }
    private void findSprite()
    {
        isSprite = 0;

        for (int i = 0; i < childList.Length; i++)
        {
            if (childList[i].GetComponent<SpriteRenderer>() != null)
            {

                m_spr.Add(childList[i].GetComponent<SpriteRenderer>());
                m_alphaNormal.Add( m_spr[isSprite].color);
                m_alphaTranslucent.Add(m_spr[isSprite].color);

                isSprite += 1;
            }
        }



        for (int x = 0; x < isSprite; x++) 
        {
            m_alphaTranslucent[x] = m_isTranslucent;
            m_isTranslucent.a = Transparency;
            m_alphaTranslucent[x] = m_isTranslucent;
        }

    }

    void Update()
    {
        if (isHit)
        {
            alphaControl();
            isHit = false;
        }
    }

    private void alphaControl()
    {
        for (int i = 0; i < isSprite; i++)
        {
            m_spr[i].color = m_alphaTranslucent[i];
            Invoke("alphaReturn", ReturnTime);
        }
    }

    private void alphaReturn()
    {
        for (int i = 0; i < isSprite; i++)
        {
            m_spr[i].color = m_alphaNormal[i];

        }
    }
}
