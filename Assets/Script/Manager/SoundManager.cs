using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource m_backGroundSound;
    [SerializeField] private AudioSource m_btnAudioSource;
    [SerializeField] private AudioMixer m_mixer;
    [SerializeField] private AudioClip m_backGroundClip;


    [SerializeField] Slider slider;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine("bgStart");
        //m_backGroundSound.PlayOneShot(오디오,클립) 무조건한번 실행
        //slider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("Master", x); });//슬라이더연결
        
    }



    IEnumerator bgStart()
    {
        yield return new WaitForSeconds(3);
        bgSoundPlay(m_backGroundClip);
    }

    public void BackGroundVolume(float _volume)
    {
        m_mixer.SetFloat("BackGround", Mathf.Log10(_volume) * 20);
    }

    public void SFXVolume(float _volume)
    {
        m_mixer.SetFloat("SFX", Mathf.Log10(_volume) * 20);
    }


  

    public void SFXPlay(AudioClip clip)
    {
        m_btnAudioSource.outputAudioMixerGroup = m_mixer.FindMatchingGroups("SFX")[0];
        m_btnAudioSource.clip = clip;
        m_btnAudioSource.loop = false;
        m_btnAudioSource.volume = 0.5f;
        m_btnAudioSource.Play();
        
    }

    public void bgSoundPlay(AudioClip clip)
    {
        m_backGroundSound.outputAudioMixerGroup = m_mixer.FindMatchingGroups("BackGround")[0];
        m_backGroundSound.clip = clip;
        m_backGroundSound.loop = true;
        m_backGroundSound.volume = 1f;
        m_backGroundSound.Play();
    }
}
