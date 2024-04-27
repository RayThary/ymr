using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource m_backGroundSound;
    [SerializeField] private AudioSource m_SFXAudioSource;
    [SerializeField] private AudioMixer m_mixer;
    [SerializeField] private AudioClip m_backGroundClip;


    [SerializeField] private Slider BackGroundSlider;
    [SerializeField] private Slider SFXSlider;



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
        //m_backGroundSound.PlayOneShot(�����, Ŭ��) �������ѹ� ����
        //slider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("Master", x); });//�����̴�����
        BackGroundSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("BackGround", Mathf.Log10(x) * 20); });
        SFXSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("SFX", x); });

    }
    //private void


    IEnumerator bgStart()
    {
        yield return new WaitForSeconds(2);
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




    /// <summary>
    /// �ܺο��� �ҷ��� clip �� �־ ����ϸ�ȴ�. 
    /// </summary>
    /// <param name="clip">���� �Ҹ�</param>
    /// <param name="_volum">�Ҹ��� ũ��</param>
    public void SFXPlay(AudioClip clip, float _volum)
    {
        m_SFXAudioSource.outputAudioMixerGroup = m_mixer.FindMatchingGroups("SFX")[0];
        m_SFXAudioSource.clip = clip;
        m_SFXAudioSource.loop = false;
        m_SFXAudioSource.volume = _volum;
        m_SFXAudioSource.Play();

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
