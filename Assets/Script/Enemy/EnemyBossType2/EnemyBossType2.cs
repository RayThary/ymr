using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemyBossType2 : Unit
{

    private Transform target;
    private Animator anim;
    private BoxCollider box;
    private float attsp = 0.5f;//공격속도

    //메테오공격(패턴1)
    private GameObject patten1ObjCheck = null;

    private Transform trsArea;//스테이지위치를저장해줄필요가있음

    private Vector3 meteorBoxSize;
    private Vector3 spawnPos;

    private float timer = 0.0f;
    private bool allSpawnCheck = false;
    //패턴2(근접주위빙글빙글도는 오브젝트) 
    [SerializeField] private GameObject patten2ObjCheck = null;

    [SerializeField] private float patten2DurationTime = 4;//패턴2지속시간
    private float patten2Timer = 0.0f;//패턴2 타이머
    private bool patten2MotionCheck = false;

    private bool patten2ObjSpawnCheck = false;


    //패턴3(반피부터 풍차모양으로 도는 레이저)
    public GameObject objPatten3;//임시 반피되면쓰는패턴 

    private GameObject patten3ObjCheck = null;

    //패턴4 (큰총알소환 4방향으로 총알날라감)
    private bool basicAttackCheck = true;
    private int attackType;
    private bool bigBulletCheck = false;

    [SerializeField] private float basicAttackTime = 6;
    private float basicAttackTimer = 0.0f;

    [SerializeField] private bool noMeleeCheck = false;

    //이동용
    private NavMeshAgent nav;
    private bool noMove = false;

    private Transform playerTrs;
    private Player player;

    private bool patten1Check = false;//메테오 패턴1써보는곳
    private bool patten2Check = false;//근접패턴 적이오면 주위구체3개소환하는곳
    private bool patten3Check = false;//반피패턴 x자로 레이저가빙빙돌게된다
    private bool patten4Check = false;//큰총알패턴

    private bool deathCheck = false;

    private bool allEnemyObjRemove = true;

    [SerializeField] private bool testHpSet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player.Hit(this, 1);
        }
    }

    protected new void Start()
    {
        base.Start();
        nav = transform.parent.GetComponent<NavMeshAgent>();
        playerTrs = GameManager.instance.GetPlayerTransform;
        player = GameManager.instance.GetPlayer;
        target = GameManager.instance.GetPlayerTransform;
        anim = GetComponent<Animator>();
        meteorBoxSize = GameManager.instance.GetStage.size;
        box = GetComponent<BoxCollider>();

        BossUI.Instance.StatBoss = stat;

        float hp = 100;
        if (GameManager.instance.GetStageNum == 1)
        {
            hp = 100;
        }
        else if (GameManager.instance.GetStageNum == 2)
        {
            hp = 150;
        }
        else
        {
            hp = 100;
            Debug.LogError($"StageNumError , StageNum ={GameManager.instance.GetStageNum}");
        }

        if (testHpSet)
        {
            hp = 3;
        }

        stat.SetHp(hp);


    }


    void Update()
    {
        if (GameManager.instance.GetStart() == false)
        {
            return;
        }


        if (deathCheck)
        {
            box.enabled = false;
            return;
        }

        enemyMove();
        enemyAttack();//기본공격
        bigBullet();
        enemyMeleeAttack();
        enemyHalfHealthPatten();
        enemyDie();
    }

    private void enemyMove()
    {

        if (noMove)
        {
            anim.SetFloat("RunState", 0);
            nav.enabled = false;

        }
        else
        {

            float playerDistance = Vector3.Distance(transform.position, target.position);
            if (playerDistance >= 4)
            {
                anim.SetFloat("RunState", 0.5f);
                nav.enabled = true;
                nav.SetDestination(target.position);
            }
            else
            {
                nav.enabled = false;
                anim.SetFloat("RunState", 0);
            }
        }
    }


    private void enemyAttack()
    {

        float dis = Vector3.Distance(transform.position, target.position);
        if (basicAttackCheck)
        {
            if (dis < 3 && patten2ObjCheck == null)
            {
                patten2Check = true;
                basicAttackCheck = false;
                basicAttackTime = 2;
            }


            attackType = Random.Range(0, 2);
            if (attackType == 0)
            {
                patten1Check = true;
                meteorAttack();
                basicAttackCheck = false;
                basicAttackTime = 6;
                if (patten3Check)
                {
                    basicAttackTime = 4;
                }
            }
            else if (attackType == 1)
            {
                bigBulletCheck = true;
                bigBulletAttack();
                basicAttackCheck = false;
                basicAttackTime = 5;
                if (patten3Check)
                {
                    basicAttackTime = 3;
                }
            }


        }
        else
        {
            if (dis < 3)
            {
                basicAttackTime = 2;
            }
            else
            {
                basicAttackTimer += Time.deltaTime;
            }
            if (basicAttackTimer >= basicAttackTime)
            {
                basicAttackTimer = 0;
                basicAttackCheck = true;
            }
        }

    }

    private void meteorAttack()
    {

        if (patten1Check)
        {
            noMeleeCheck = true;
            attsp = 0.5f;
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState", 0);
            anim.SetFloat("NormalState", 1);
            anim.SetFloat("AttackSpeed", attsp);


            allSpawnCheck = false;
            patten1Check = false;

            StartCoroutine(patten1Spawn());
        }

    }

    IEnumerator patten1Spawn()
    {
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(patten1());
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator patten1()
    {
        for (int i = 0; i < 9; i++)
        {
            spawnPos = GetRandomPosition();
            patten1ObjCheck = PoolingManager.Instance.CreateObject("Meteor", GameManager.instance.GetEnemyAttackObjectPatten);
            patten1ObjCheck.transform.position = spawnPos;
            patten1ObjCheck.GetComponent<Meteor>().Boss = this;

            if (i % 2 == 0)
            {
                yield return new WaitForSeconds(0.5f);
            }

            if (i == 8)
            {
                allSpawnCheck = true;
            }
        }
    }


    private Vector3 GetRandomPosition()
    {
        Vector3 basicPos = new Vector3(14.5f, 0, 14.5f);
        Vector3 size = meteorBoxSize;




        int count = 0;
        float posX = basicPos.x + Random.Range(-size.x / 2, size.x / 2);
        float posZ = basicPos.x + Random.Range(-size.z / 2, size.z / 2);
        Vector3 spawnVec = new Vector3(posX, 0, posZ);
        float meteorDis = Vector3.Distance(target.position, spawnVec);

        while (meteorDis > 5)
        {
            posX = basicPos.x + Random.Range(-size.x / 2, size.x / 2);
            posZ = basicPos.x + Random.Range(-size.z / 2, size.z / 2);
            spawnVec = new Vector3(posX, 0, posZ);
            meteorDis = Vector3.Distance(target.position, spawnVec);
            count++;
            if (count > 10)
            {
                break;
            }
        }

        return spawnVec;
    }

    private void bigBullet()
    {
        if (patten4Check)
        {
            StartCoroutine(bigBulletSpawn());
            patten4Check = false;
        }
    }

    private void bigBulletAttack()
    {

        if (bigBulletCheck)
        {
            noMeleeCheck = true;
            attsp = 0.3f;
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState", 1);
            anim.SetFloat("SkillState", 1);
            anim.SetFloat("AttackSpeed", attsp);
            bigBulletCheck = false;
        }

    }

    IEnumerator bigBulletSpawn()
    {
        GameObject bigBulletObj;
        bigBulletObj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BigBullet, GameManager.instance.GetEnemyAttackObjectPatten);
        bigBulletObj.GetComponent<BigBulletPatten>().Boss = this;
        bigBulletObj.transform.position = transform.position;
        bigBulletObj.GetComponent<BigBulletPatten>().Operation();
        Vector3 player = playerTrs.transform.position - transform.position;

        bigBulletObj.transform.rotation = Quaternion.LookRotation(player);
        yield return new WaitForSeconds(10);
        if (bigBulletObj != null)
        {
            PoolingManager.Instance.RemovePoolingObject(bigBulletObj);
        }
    }
    private void enemyMeleeAttack()
    {


        if (patten2ObjCheck != null)
        {
            patten2Timer += Time.deltaTime;
            if (patten2Timer >= patten2DurationTime)
            {
                PoolingManager.Instance.RemovePoolingObject(patten2ObjCheck);
                patten2ObjCheck = null;
                patten2Timer = 0;
                patten2MotionCheck = false;
            }
        }
        else
        {

            if (patten2Check)
            {
                if (patten2MotionCheck == false)
                {
                    attsp = 0.5f;

                    anim.SetTrigger("Attack");
                    anim.SetFloat("AttackState", 0);
                    anim.SetFloat("NormalState", 0.5f);
                    anim.SetFloat("AttackSpeed", attsp);
                    patten2MotionCheck = true;
                    patten2Check = false;
                }
            }


            if (patten2ObjSpawnCheck)
            {
                Vector3 spawnPos = new Vector3(transform.position.x, 0.1f, transform.position.z + 1);
                patten2ObjCheck = PoolingManager.Instance.CreateObject("RotatingSphere", transform.parent);
                patten2ObjCheck.transform.position = spawnPos;
                patten2ObjCheck.GetComponentInChildren<Type2Patten2RedStart>().setType2(this);
                patten2ObjSpawnCheck = false;
            }
        }
    }

    private void enemyHalfHealthPatten()
    {

        if (patten3Check)
        {
            return;
        }
        if (stat.HP <= stat.MAXHP / 2)
        {
            patten3ObjCheck = PoolingManager.Instance.CreateObject("WindMillPatten", GameManager.instance.GetEnemyAttackObjectPatten);
            patten3Check = true;
            patten3ObjCheck.GetComponent<WindMillPattenUnit>().Boss = this;
        }

    }

    private void enemyDie()
    {
        if (stat.HP <= 0)
        {
            deathCheck = true;

            anim.SetTrigger("Die");

            patten2ObjCheck = null;
            patten3ObjCheck = null;

            PoolingManager.Instance.RemoveAllPoolingObject(GameManager.instance.GetEnemyAttackObjectPatten.gameObject);

            SoundManager.instance.bgSoundPause();
            GameObject potal = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.NextStage, null);
            potal.transform.position = new Vector3(14.5f, 0, 14.5f);
        }
    }



    //애니메이션용

    private void enemyAttackMotionStart()
    {
        noMove = true;
    }
    private void enemyAttackMotionEnd()
    {
        noMove = false;
        noMeleeCheck = false;
    }

    private void bigBulletMotion()
    {
        patten4Check = true;
    }

    private void meleeAttack()
    {
        patten2ObjSpawnCheck = true;

    }



}
