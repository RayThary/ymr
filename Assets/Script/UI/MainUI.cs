using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{

    private GameObject m_btnParent;

    private Button m_btnStart;
    private Button m_btnOption;
    private Button m_btnOptionClose;
    private Button m_btnControlKey;
    private Button m_btnControlKeyClose;
    private Button m_btnEnd;


    private GameObject m_optinWindow;
    private GameObject m_controlKeyWindow;

    void Start()
    {
        m_btnParent = transform.Find("Button").gameObject;

        m_optinWindow = transform.Find("SoundWindow").gameObject;
        m_controlKeyWindow = transform.Find("ControlKeyWindow").gameObject;

        m_btnStart = m_btnParent.transform.Find("Start").GetComponent<Button>();
        m_btnOption = m_btnParent.transform.Find("Option").GetComponent<Button>();
        m_btnControlKey = m_btnParent.transform.Find("ControlKey").GetComponent<Button>();
        m_btnEnd = m_btnParent.transform.Find("Exit").GetComponent<Button>();

        m_btnOptionClose = m_optinWindow.transform.Find("OptionClose").GetComponent<Button>();
        m_btnControlKeyClose = m_controlKeyWindow.transform.Find("OptionClose").GetComponent<Button>();  

        m_btnStart.onClick.AddListener(btnStart);
        m_btnOption.onClick.AddListener(btnOption);
        m_btnControlKey.onClick.AddListener(btnControlKey);
        m_btnEnd.onClick.AddListener(btnEnd);

        m_btnOptionClose.onClick.AddListener(btnOptionClose);
        m_btnControlKeyClose.onClick.AddListener(btnControlKeyClose);

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
            SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
            m_optinWindow.SetActive(true);
        }
    }

    private void btnOptionClose()
    {
        if (m_optinWindow.activeSelf == true)
        {
            SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
            m_optinWindow.SetActive(false);
        }
    }

    private void btnControlKey()
    {
        if (m_controlKeyWindow.activeSelf == false)
        {
            SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
            m_controlKeyWindow.SetActive(true);
        }
    }

    private void btnControlKeyClose()
    {
        if (m_controlKeyWindow.activeSelf == true)
        {
            SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
            m_controlKeyWindow.SetActive(false);
        }
    }

    private void btnEnd()
    {
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
        Application.Quit();
    }
}
