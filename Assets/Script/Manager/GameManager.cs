using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private CardManager cardManager;
    public static GameManager instance;

    private Player player;
    public Player GetPlayer { get { if (player == null) { player = FindObjectOfType<Player>(); } return player; } }
    public Transform GetPlayerTransform { get { return player.transform; } }

    private Transform enemyAttackObj;
    public Transform GetEnemyAttackObjectParent { get { return enemyAttackObj; } }

    public Transform GetSFXParent { get { return transform.GetChild(1); } }

    public Transform GetPlayerBulletParent { get { return transform.GetChild(2); } }

    public bool CardSelect = false;

    public Transform GetenemyObjectBox { get { return transform.GetChild(0); } }

    public GameObject playerDeadButton;

    [SerializeField] private BoxCollider stage;
    public BoxCollider GetStage { get { if (stage == null) { stage = GetComponentInChildren<BoxCollider>(); } return stage; } }

    [SerializeField] private int stageNum = 1;
    public int GetStageNum { get { return stageNum; } }

    private bool StartCheck = true;

    private List<int> stageList = new List<int>();

    private string bossName;
    public string BossName { get { return bossName; } }

    [SerializeField]
    //카드 고를때 화면을 가릴 이미지
    private Image cardSelectWindow;

    //튜토리얼에서 무기를 고르지 않고 포탈에 들어갈 경우 메세지
    [SerializeField]
    private GameObject waringText;

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

        DontDestroyOnLoad(this);

        player = FindObjectOfType<Player>();
        enemyAttackObj = transform.GetChild(0);
        stage = GetComponentInChildren<BoxCollider>();

        //프레임 설정 / 화면비
        RefreshRate rate = new RefreshRate();
        rate.numerator = 120;
        Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, rate);
        //프레임 제한 최대 60
        Application.targetFrameRate = 60;//59 

        for (int i = 1; i < 3; i++)
        {
            stageList.Add(i);
        }
    }

    void Start()
    {

    }

    private void Update()
    {

        if (CardSelect)
        {
            CardSelect = false;

            if (cardManager == null)
            {
                cardManager = CardManager.Instance;
            }

            cardManager.ViewCards();

        }
    }

    //보스가 죽고 포탈에 들어가면 실행
    public void CardSelectStep()
    {
        if (stageNum == 4)
        {
            CardSelected();
        }
        else
        {
            //근데 무기를 장착중인지 확인 해야함
            WeaponDepot w = player.GetComponent<WeaponDepot>();
            //장착하고 있는 무기의 갯수가 0개가 아니다!
            if (w.Launcher.AttackTypes.Count != 0)
            {
                float width = Screen.width;
                float height = Screen.height;
                if (cardSelectWindow == null)
                {
                    cardSelectWindow = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.CardSelectWindow, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
                    cardSelectWindow.transform.position = new Vector3(width * 0.5f, height * 0.5f);
                    cardSelectWindow.transform.SetAsFirstSibling();
                }

                //화면 가려주고
                cardSelectWindow.rectTransform.sizeDelta = new Vector2(width, height);
                //카드 보여주고
                CardSelect = true;
            }
            else
            {
                StartCoroutine(WaringText());
            }
        }
    }
    private IEnumerator WaringText()
    {
        waringText.SetActive(true);
        yield return new WaitForSeconds(2);
        waringText.SetActive(false);
    }

    public void CardSelected()
    {
        NextStageStep();
        if (cardSelectWindow != null)
            cardSelectWindow.rectTransform.sizeDelta = new Vector2(0, 0);
    }

    //CardSelected 작동한 후에 실행
    public void NextStageStep()
    {
        //화면 다시 열어주고
        if (cardSelectWindow != null)
            cardSelectWindow.rectTransform.sizeDelta = new Vector2(0, 0);
        //스테이지 중에 랜덤으로 하나 골라서 씬 로드
        if (stageNum == 4)
        {
            GetPlayer.InputKey = false;
            SceneManager.LoadScene("EndScene");
        }
        else if (stageNum == 3)
        {
            //마지막 보스 스테이지 로드
            SceneManager.LoadScene("Type" + "End" + "Stage");
        }
        else
        {
            int stage = stageList[Random.Range(0, stageList.Count)];
            stageList.Remove(stage);
            SceneManager.LoadScene("Type" + stage + "Stage");
        }

        //여기서 선택한 씬을 기록

        //씬넘어갈때 플레이어 총알 부모의자식들 삭제
        PoolingManager.Instance.RemoveAllPoolingObject(GetPlayerBulletParent.gameObject);

    }

    public Collider NearbyTrnasform(Collider[] list, Transform center)
    {
        if (list.Length == 0)
            return null;
        int index = 0;
        float min = Vector3.Distance(center.position, list[0].transform.position);
        for (int i = 1; i < list.Length; i++)
        {
            float distnace = Vector3.Distance(center.position, list[i].transform.position);
            if (distnace < min)
            {
                min = distnace;
                index = i;
            }
        }
        return list[index];
    }

    public void SetStageNum()
    {
        stageNum++;
    }

    public void SetStart(bool _value)
    {
        StartCheck = _value;
    }

    public bool GetStart()
    {
        return StartCheck;
    }



    public void PlayerDead()
    {
        GameStop();
        playerDeadButton.SetActive(true);
    }

    public void GameStop()
    {
        Time.timeScale = 0;
    }
    public void GamePlay()
    {
        Time.timeScale = 1;
    }

    public void MainMenitScenesLoad()
    {
        if (GetPlayer != null)
        {
            SceneManager.sceneLoaded -= GetPlayer.OnSceneLoaded;
            for (int i = 0; i < cardManager.SelectCards.Count; i++)
            {
                cardManager.SelectCards[i].Deactivation();
            }
            cardManager.SelectCards.Clear();
            Destroy(GetPlayer.gameObject);
        }
        SceneManager.LoadScene("MainScene");

        for (int i = 1; i < 3; i++)
        {
            stageList.Add(i);
        }
    }

    public void ExitGame()
    {
        //Debug.Log("종료 코드");
        Application.Quit();
    }

    public void RestartGame()
    {
        //NextStageStep 에서 선택한 씬을 다시 로드 
        //0이나 1인 경우 플레이어를 삭제 해야함
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (GetPlayer != null)
            {
                SceneManager.sceneLoaded -= GetPlayer.OnSceneLoaded;
                Destroy(GetPlayer.gameObject);
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //플레이어의 스탯을 다시 로드
        player.STAT.Init();
    }

}