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
        dialogue.Add("모든 차원을 돌아다니며 적을 해치운 주인공은");
        dialogue.Add("다시 본래의 차원으로 돌아가");
        dialogue.Add("평화로운 생활을 즐기고 있지만");
        dialogue.Add("다시 적이 나타나면 언제나 해치우러 갈 준비가 되어있다.");

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
