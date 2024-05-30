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
    //ī�� ���� ȭ���� ���� �̹���
    private Image cardSelectWindow;

    //Ʃ�丮�󿡼� ���⸦ ���� �ʰ� ��Ż�� �� ��� �޼���
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

        //������ ���� / ȭ���
        RefreshRate rate = new RefreshRate();
        rate.numerator = 120;
        Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, rate);
        //������ ���� �ִ� 60
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

    //������ �װ� ��Ż�� ���� ����
    public void CardSelectStep()
    {
        if (stageNum == 4)
        {
            CardSelected();
        }
        else
        {
            //�ٵ� ���⸦ ���������� Ȯ�� �ؾ���
            WeaponDepot w = player.GetComponent<WeaponDepot>();
            //�����ϰ� �ִ� ������ ������ 0���� �ƴϴ�!
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

                //ȭ�� �����ְ�
                cardSelectWindow.rectTransform.sizeDelta = new Vector2(width, height);
                //ī�� �����ְ�
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

    //CardSelected �۵��� �Ŀ� ����
    public void NextStageStep()
    {
        //ȭ�� �ٽ� �����ְ�
        if (cardSelectWindow != null)
            cardSelectWindow.rectTransform.sizeDelta = new Vector2(0, 0);
        //�������� �߿� �������� �ϳ� ��� �� �ε�
        if (stageNum == 4)
        {
            GetPlayer.InputKey = false;
            SceneManager.LoadScene("EndScene");
        }
        else if (stageNum == 3)
        {
            //������ ���� �������� �ε�
            SceneManager.LoadScene("Type" + "End" + "Stage");
        }
        else
        {
            int stage = stageList[Random.Range(0, stageList.Count)];
            stageList.Remove(stage);
            SceneManager.LoadScene("Type" + stage + "Stage");
        }

        //���⼭ ������ ���� ���

        //���Ѿ�� �÷��̾� �Ѿ� �θ����ڽĵ� ����
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
        //Debug.Log("���� �ڵ�");
        Application.Quit();
    }

    public void RestartGame()
    {
        //NextStageStep ���� ������ ���� �ٽ� �ε� 
        //0�̳� 1�� ��� �÷��̾ ���� �ؾ���
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (GetPlayer != null)
            {
                SceneManager.sceneLoaded -= GetPlayer.OnSceneLoaded;
                Destroy(GetPlayer.gameObject);
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //�÷��̾��� ������ �ٽ� �ε�
        player.STAT.Init();
    }

}