using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource m_masterSource;
    [SerializeField] private AudioSource m_backGroundSource;
    
    [SerializeField] private AudioMixer m_mixer;

    [SerializeField] private AudioClip m_battleBackGroundClip;
    [SerializeField] private AudioClip m_mainBackGroundClip;

    [SerializeField] private Slider MasterSoundSlider;
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


      
        MasterSoundSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("Master", Mathf.Log10(x) * 20); });//슬라이더연결
        //BackGroundSlider.onValueChanged.AddListener((x) => { m_backGroundSource.volume = x; });//슬라이더연결
        BackGroundSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("BackGround", Mathf.Log10(x) * 20); });//슬라이더연결
        SFXSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("SFX", Mathf.Log10(x) * 20); });//슬라이더연결
        
        //SFXSlider.onValueChanged.AddListener((x) => { m_SFXAudioSource.volume = x; });

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
    /// 외부에서 불러서 clip 을 넣어서 사용하면된다. 
    /// </summary>
    /// <param name="clip">사용될 소리</param>
    /// <param name="_volum">소리의 크기</param>
    public void SFXPlay(AudioClip clip, float _volum)
    {
        StartCoroutine(SFXPlaying(clip, _volum));
    }

    IEnumerator SFXPlaying(AudioClip clip, float _volum)
    {
        GameObject SFXSource = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.SFXAuiodSource, GameManager.instance.GetSFXParent);

        AudioSource m_sfxaudiosource = SFXSource.GetComponent<AudioSource>();

        m_sfxaudiosource.outputAudioMixerGroup = m_mixer.FindMatchingGroups("sfx")[0];
        m_sfxaudiosource.clip = clip;
        m_sfxaudiosource.loop = false;
        m_sfxaudiosource.volume = _volum;
        m_sfxaudiosource.Play();
        yield return new WaitForSeconds(clip.length);
        PoolingManager.Instance.RemovePoolingObject(SFXSource);
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
