using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CardManager cardManager;
    public static GameManager instance;

    private Player player;
    public Player GetPlayer { get { return player; } }    
    public Transform GetPlayerTransform { get { return player.transform; } }

    private Transform enemyAttackObj;
    public Transform GetEnemyAttackObjectPatten { get { return enemyAttackObj; } }

    public bool CardTest = false;

    public Transform GetenemyObjectBox { get { return transform.GetChild(0); } }

    public GameObject playerDeadButton;

    [SerializeField] private BoxCollider stage;
    public BoxCollider GetStage { get { return stage; } }

    [SerializeField]private int stageNum = 1;
    public int GetStageNum { get { return stageNum; } }

    private bool StartCheck = true;

    private List<int> stageList = new List<int>();

    [SerializeField]
    //ī�� ���� ȭ���� ���� �̹���
    private Image cardSelectWindow;

    private bool bossDieCheck = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
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

        for (int i = 1; i < 5; i++)
        {
            stageList.Add(i);
        }
    }

    void Start()
    {
       
    }

    private void Update()
    {
        if(CardTest)
        {
            CardTest = false;
            cardManager.ViewCards();
        }

        if (bossDieCheck)
        {
            PoolingManager.Instance.RemoveAllPoolingObject(transform.GetChild(0).gameObject);
            bossDieCheck = false;
        }

     
    }

    //������ ������ ����
    public void CardSelectStep()
    {
        //�ٵ� ���⸦ ���������� Ȯ�� �ؾ���
        WeaponDepot w = player.GetComponent<WeaponDepot>();
        //�����ϰ� �ִ� ������ ������ 0���� �ƴϴ�!
        if (w.Launcher.AttackTypes.Count != 0)
        {
            //ȭ�� �����ְ�
            cardSelectWindow.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            //ī�� �����ְ�
            CardTest = true;
        }
    }

    public void CardSelected()
    {
        NextStageStep();
    }

    //CardSelected �۵��� �Ŀ� ����
    public void NextStageStep()
    {
        //ȭ�� �ٽ� �����ְ�
        cardSelectWindow.rectTransform.sizeDelta = new Vector2(0, 0);
        //�������� �߿� �������� �ϳ� ��� �� �ε�
        int stage = stageList[Random.Range(0, stageList.Count)];
        stageList.Remove(stage);
        //SceneManager.LoadScene("BossType" + stage);
        SceneManager.LoadScene(1);
        //���⼭ ������ ���� ���
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

    public void SetBossDieCheck(bool _value)
    {
        bossDieCheck = _value;
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
        Time.timeScale = 0;
        playerDeadButton.SetActive(true);
    }


    public void ExitGame()
    {
        //Debug.Log("���� �ڵ�");
        Application.Quit();
    }

    public void RestartGame()
    {
        //NextStageStep ���� ������ ���� �ٽ� �ε�

        //�÷��̾��� ������ �ٽ� �ε�
        player.STAT.Init();
    }

}
