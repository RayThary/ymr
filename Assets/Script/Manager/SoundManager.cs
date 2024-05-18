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

    public enum Clips
    {
        Arrow,
    }

    [SerializeField] private AudioSource m_backGroundSource;

    [SerializeField] private AudioMixer m_mixer;

    [SerializeField] private AudioClip m_battleBackGroundClip;
    [SerializeField] private AudioClip m_mainBackGroundClip;

    [SerializeField]private GameObject optionObj;

    //없어지면찾아주는거 설정해줄필요있음
    [SerializeField] private Slider MasterSoundSlider;
    [SerializeField] private Slider BackGroundSlider;
    [SerializeField] private Slider SFXSlider;

    private float masterVolum;
    private float backVolum;
    private float SFXVolum;


    [SerializeField] private List<AudioClip> clips = new List<AudioClip>();



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

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        m_backGroundSource = GetComponent<AudioSource>();
        optionObj = GameObject.Find("Canvas");
        Transform option = optionObj.transform.Find("SoundWindow");

        MasterSoundSlider = option.Find("MasterSound").GetComponentInChildren<Slider>();
        BackGroundSlider= option.Find("BackGroundSound").GetComponentInChildren<Slider>();
        SFXSlider = option.Find("SFXSound").GetComponentInChildren<Slider>();

        StartCoroutine("bgStart");



        MasterSoundSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("Master", Mathf.Log10(x) * 20); });//슬라이더연결
        BackGroundSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("BackGround", Mathf.Log10(x) * 20); });//슬라이더연결
        SFXSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("SFX", Mathf.Log10(x) * 20); });//슬라이더연결



    }

    private void Update()
    {
        if (optionObj == null)
        {
            optionObj = GameObject.Find("Canvas");
            if (optionObj == null)
            {
                Debug.LogError("캔버스가없음");
            }

            Transform option = optionObj.transform.Find("SoundWindow");

            if (option == null)
            {
                Debug.LogError("SoundWindow");
            }

            MasterSoundSlider = option.Find("MasterSound").GetComponentInChildren<Slider>();
            BackGroundSlider = option.Find("BackGroundSound").GetComponentInChildren<Slider>();
            SFXSlider = option.Find("SFXSound").GetComponentInChildren<Slider>();

            MasterSoundSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("Master", Mathf.Log10(x) * 20); });//슬라이더연결
            BackGroundSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("BackGround", Mathf.Log10(x) * 20); });//슬라이더연결
            SFXSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("SFX", Mathf.Log10(x) * 20); });//슬라이더연결
        }

        masterVolum = MasterSoundSlider.value;
        backVolum = BackGroundSlider.value;
        SFXVolum = SFXSlider.value;
    }


    IEnumerator bgStart()
    {
        yield return new WaitForSeconds(0.5f);
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (m_backGroundSource.clip != m_mainBackGroundClip)
            {
                bgSoundPlay(m_mainBackGroundClip);
            }

        }
        else
        {
            bgSoundPlay(m_battleBackGroundClip);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(bgStart());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 외부에서 불러서 clip 을 넣어서 사용하면된다. 
    /// </summary>
    /// <param name="clip">사용될 소리</param>
    /// <param name="_volum">소리의 크기</param>
    public void SFXCreate(Clips _clip, float _volum)
    {
        AudioClip clip = clips.Find(x => x.name == _clip.ToString());

        SFXPlay(clip, _volum);
    }

    private void SFXPlay(AudioClip clip, float _volum)
    {
        StartCoroutine(SFXPlaying(clip, _volum));
    }

    IEnumerator SFXPlaying(AudioClip clip, float _volum)
    {
        GameObject SFXSource = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.SFXSource, GameManager.instance.GetSFXParent);

        AudioSource m_sfxaudiosource = SFXSource.GetComponent<AudioSource>();

        m_sfxaudiosource.outputAudioMixerGroup = m_mixer.FindMatchingGroups("SFX")[0];
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

    public void bgSoundPause()
    {
        m_backGroundSource.Pause();
    }

    public void soundrVolums(float _master, float _backGround, float _SFX)
    {
        _master = masterVolum;
        _backGround = backVolum;
        _SFX = SFXVolum;
    }

}
