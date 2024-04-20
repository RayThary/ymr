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
        //Ʃ�丮�� �������� �ε�
        SceneManager.LoadScene("Tutorial");
    }
    public void OptionButton()
    {
        //����â ����
        optionWindow.SetActive(true);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}