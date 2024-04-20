using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScript : MonoBehaviour
{
    [SerializeField]
    private GameObject optionWindow;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartButton()
    {
        //튜토리얼 스테이지 로드
        SceneManager.LoadScene("Tutorial");
    }
    public void OptionButton()
    {
        //설정창 열기
        optionWindow.SetActive(true);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}