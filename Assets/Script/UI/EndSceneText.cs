using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSceneText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI story;
    private List<string> dialogue = new ();

    [SerializeField]
    private TextMeshProUGUI anykey;

    // Start is called before the first frame update
    void Start()
    {
        dialogue.Add("��� ������ ���ƴٴϸ� ���� ��ġ�� ���ΰ���");
        dialogue.Add("�ٽ� ������ �������� ���ư�");
        dialogue.Add("��ȭ�ο� ��Ȱ�� ���� ������");
        dialogue.Add("�ٽ� ���� ��Ÿ���� ������ ��ġ�췯 �� �غ� �Ǿ��ִ�.");

        StartCoroutine(Texting());
    }

    private void Update()
    {
        if(anykey.gameObject.activeSelf)
        {
            if(Input.anyKey)
            {
                GameManager.instance.MainMenitScenesLoad();
            }
        }
    }

    IEnumerator Texting()
    {
        StartCoroutine(FadeIn(story, dialogue[0]));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(FadeOut(story));
        yield return new WaitForSeconds(2);


        StartCoroutine(FadeIn(story, dialogue[1]));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(FadeOut(story));
        yield return new WaitForSeconds(2);


        StartCoroutine(FadeIn(story, dialogue[2]));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(FadeOut(story));
        yield return new WaitForSeconds(2);


        StartCoroutine(FadeIn(story, dialogue[3]));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(FadeOut(story));
        yield return new WaitForSeconds(2);

        anykey.gameObject.SetActive(true);
    }

    IEnumerator FadeIn(TextMeshProUGUI textMeshPro, string str)
    {
        textMeshPro.text = str;
        while (textMeshPro.color.a < 1)
        {
            yield return null;
            textMeshPro.color += new Color(0,0,0, Time.deltaTime * 0.7f);
        }
    }

    IEnumerator FadeOut(TextMeshProUGUI textMeshPro)
    {
        while (textMeshPro.color.a > 0)
        {
            yield return null;
            textMeshPro.color -= new Color(0, 0, 0, Time.deltaTime * 0.7f);
        }
    }
}
