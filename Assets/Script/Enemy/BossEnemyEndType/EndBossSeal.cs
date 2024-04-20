using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBossSeal : MonoBehaviour
{
    //테스트
    [SerializeField] private bool halfPattenCheck;
    //기본스킬들 재사용 시간
    [SerializeField] private float basicAttackTime = 2.5f;
    private float basicAttackTimer = 3;
    [SerializeField] private int pattenNum = 4;
    private bool colorChange = false;

    [SerializeField] private float UpGroundRange = 2;//반지름

    [SerializeField] private float upGroundTime = 1;

    [SerializeField] private SpriteRenderer spr;
    private bool upGroundPattening = false;

    [SerializeField] private float rotationSpeed = 120;

    //외부참조
    private Transform playerTrs;
    private Unit boss;
    public Unit Boss { set => boss = value; }

    void Start()
    {

        spr = GetComponentInChildren<SpriteRenderer>();
        playerTrs = GameManager.instance.GetPlayerTransform;
    }

    // Update is called once per frame
    void Update()
    {
        attackPatten();
        sealRotation();

    }


    private void attackPatten()
    {
        if (boss.STAT.HP > boss.STAT.MAXHP * 0.9f)
        {
            return;
        }
        basicAttackTimer += Time.deltaTime;
        if (basicAttackTimer >= basicAttackTime - 0.5f && colorChange == false)
        {
            int attack = Random.Range(0, 10);
            if (attack < 7)
            {
                pattenNum = 4;
                colorChange = true;
            }
            else
            {
                float nowHp = boss.STAT.HP;
                if (nowHp < boss.STAT.MAXHP * 0.3f)
                {
                    pattenNum = Random.Range(0, 3);
                }
                else if (nowHp < boss.STAT.MAXHP * 0.7f)
                {
                    pattenNum = Random.Range(0, 2);
                }
                else
                {
                    pattenNum = 0;
                }

                //레이저 블록 나비 순서
                if (pattenNum == 0)
                {
                    spr.color = Color.black;
                    colorChange = true;
                }
                else if (pattenNum == 1)
                {
                    spr.color = Color.red;
                    colorChange = true;
                }
                else if (pattenNum == 2)
                {
                    spr.color = Color.cyan;
                    colorChange = true;
                }

            }

        }
        if (basicAttackTimer >= basicAttackTime)
        {
            if (pattenNum == 0)
            {
                GameObject attackObj;
                attackObj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.LaserPatten, GameManager.instance.GetEnemyAttackObjectPatten);
                attackObj.transform.position = transform.position;
                basicAttackTimer = 0;
                colorChange = false;
            }

            else if (pattenNum == 1)
            {
                for (int i = 0; i < 8; i++)
                {
                    GameObject attackObj;
                    float rangeX = Random.Range(-UpGroundRange, UpGroundRange);
                    float rnageZ = Random.Range(-UpGroundRange, UpGroundRange);

                    Vector3 randomSpawnVec = playerTrs.position;
                    randomSpawnVec.x += rangeX;
                    randomSpawnVec.z += rnageZ;
                    attackObj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.UpGroundObj, GameManager.instance.GetEnemyAttackObjectPatten);
                    DangerZone danger = attackObj.GetComponentInChildren<DangerZone>();
                    danger.SetTime(upGroundTime);
                    attackObj.transform.position = randomSpawnVec;
                }

                basicAttackTimer = 0;
                colorChange = false;
            }
            else if (pattenNum == 2)
            {
                GameObject attackObj;
                attackObj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.RedButterfly, GameManager.instance.GetEnemyAttackObjectPatten);
                attackObj.transform.position = transform.position;
                basicAttackTimer = 0;
                colorChange = false;
            }
            else
            {
                basicAttackTimer = 0;
                colorChange = false;
            }
        }
    }

    private void sealRotation()
    {
        if (pattenNum == 4)
        {
            transform.Rotate(new Vector3(0,0,1) * rotationSpeed * Time.deltaTime);
        }
    }
    

}
