using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public void BackGroundVolume(float _volume)
    {
        //m_mixer.SetFloat("BackGround", Mathf.Log10(_volume) * 20);
        m_backGroundSource.volume = _volume;
    }

    public void SFXVolume(float _volume)
    {
        m_mixer.SetFloat("SFX", Mathf.Log10(_volume) * 20);
    }


    public static SoundManager instance;
    private AudioSource m_masterSource;
    [SerializeField] private AudioSource m_backGroundSource;
    [SerializeField] private AudioSource m_SFXAudioSource;
    [SerializeField] private AudioMixer m_mixer;

    [SerializeField] private AudioClip m_battleBackGroundClip;
    [SerializeField] private AudioClip m_mainBackGroundClip;

    [SerializeField] private Slider MasterSound;
    [SerializeField] private Slider BackGroundSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private float masterVolum;
    [SerializeField] private float backVolum;
    [SerializeField] private float SFXVolum;

    


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
        m_masterSource = GetComponent<AudioSource>();

        StartCoroutine("bgStart");
        //m_backGroundSound.PlayOneShot(�����, Ŭ��) �������ѹ� ����
        MasterSound.onValueChanged.AddListener((x) => { m_mixer.SetFloat("Master", Mathf.Log10(x) * 20); });//�����̴�����

        //BackGroundSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("BackGround", x); });
        //BackGroundSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("BackGround", Mathf.Log10(x) * 20); });
        SFXSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("SFX", x); });
        
    }

    private void Update()
    {
        masterVolum = BackGroundSlider.value;
        backVolum = BackGroundSlider.value;
        SFXVolum = SFXSlider.value;
    }


    IEnumerator bgStart()
    {
        yield return new WaitForSeconds(2);
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
        {
            bgSoundPlay(m_mainBackGroundClip);
        }
        else
        {
            bgSoundPlay(m_battleBackGroundClip);
        }
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
        m_backGroundSource.outputAudioMixerGroup = m_mixer.FindMatchingGroups("BackGround")[0];
        m_backGroundSource.clip = clip;
        m_backGroundSource.loop = true;
        m_backGroundSource.volume = 1f;
        m_backGroundSource.Play();
    }
}
