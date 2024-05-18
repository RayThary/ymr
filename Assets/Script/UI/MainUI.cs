using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{

    [SerializeField] private GameObject m_btnParent;

    [SerializeField] private Button m_btnStart;
    [SerializeField] private Button m_btnOption;
    [SerializeField] private Button m_btnOptionClose;
    [SerializeField] private Button m_btnEnd;


    private GameObject m_optinWindow;

    void Start()
    {
        m_btnParent = transform.Find("Button").gameObject;
        m_optinWindow = transform.Find("SoundWindow").gameObject;

        m_btnStart = m_btnParent.transform.Find("Start").GetComponent<Button>();
        m_btnOption = m_btnParent.transform.Find("Option").GetComponent<Button>();
        m_btnEnd = m_btnParent.transform.Find("Exit").GetComponent<Button>();
        m_btnOptionClose = m_optinWindow.transform.Find("OptionClose").GetComponent<Button>();

        m_btnStart.onClick.AddListener(btnStart);
        m_btnOption.onClick.AddListener(btnOption);
        m_btnEnd.onClick.AddListener(btnEnd);
        m_btnOptionClose.onClick.AddListener(btnOptionClose);


        m_optinWindow.SetActive(false);

    }

    private void btnStart()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void btnOption()
    {
        if (m_optinWindow.activeSelf == false)
        {
            m_optinWindow.SetActive(true);
        }
    }

    private void btnOptionClose()
    {
        if (m_optinWindow.activeSelf == true)
        {
            m_optinWindow.SetActive(false);
        }
    }

    private void btnEnd()
    {
        Application.Quit();
    }
}
