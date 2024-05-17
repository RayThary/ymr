using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static SpirteOrder;

public class BossEnemyEndType : Unit
{

    private NavMeshAgent nav;
    private Animator anim;
    private Transform target;//플레이어



    // 기본패턴 2개

    //근접패턴
    private bool vicinityPattenCheck = false;//사정거리안에있는지 밖에있는지 체크해주는부분으로만들어줌

    private bool vicinityCheck = false;//안에있을때 한번만체크해주기위한것
    private bool vicinityCoolCheck = false;//쿨타임이공격끝나고이후부터 돌기위한불값
    [SerializeField] private float vicinityCoolTime = 2;
    private float vicinityTimer = 0;

    private bool vicinityAttack = false;// 데미지를 입히는시간 
    private bool vicinityAttackRangeCheck = false; // 소환되고 사라지는시간
    private GameObject attackRange;

    //돌진 패턴
    private bool farPattenCheck = false;
    private bool farAttackStop = false;//애니메이션을 정지시켜주기위해서만듬
    private bool farAttackCheck = false;//들어오고 시간을체크한이후에 공격모션시작하는부분을위해만들어줌

    private bool farStartCheck = false;

    private float farAttackStopTimer = 0;
    private Vector3 targetVec;
    private List<GameObject> bullet = new List<GameObject>();


    [SerializeField] private BoxCollider box;//돌진시 피격판정용

    //몇초동안 멸리있었는지체크해주기위해 만듬
    [SerializeField] private float farTime = 5;
    private float farTimer = 0;


    private float noVicinityAttackTimer = 0;

    //반피이하 패턴
    private bool halfPattenCheck = false;
    private bool halfPattenIng = false;
    [SerializeField] private List<EndBossSeal> cube = new List<EndBossSeal>();

    private float halfTimer = 0;

    [SerializeField] private float moveSpeed = 3.5f;

    private bool deathCheck = false;

    private Player player;

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
        nav = GetComponentInParent<NavMeshAgent>();
        box = transform.parent.GetComponent<BoxCollider>();
        box.enabled = false;

        anim = GetComponent<Animator>();
        target = GameManager.instance.GetPlayerTransform;
        player = GameManager.instance.GetPlayer;
        anim.SetFloat("RunState", 0.5f);
        nav.speed = moveSpeed;

        cube.FindAll(x => x.Boss = this);

        BossUI.Instance.StatBoss = stat;
        
        stat.SetHp(200);//보스hp
        
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

        endBossMove();
        farPatten();
        vicinityPatten();
        HaxagonPatten();
        enemyDie();

    }

    private void endBossMove()
    {
        float dis = Vector3.Distance(transform.position, target.position);
        if (dis >= 2)
        {
            nav.SetDestination(target.position);
            if (vicinityAttackRangeCheck == false)
            {
                farTimer += Time.deltaTime;

            }


        }
        else
        {
            if (vicinityCheck == false)
            {
                vicinityPattenCheck = true;
            }
        }

        //좌우체크용
        if (transform.position.x >= target.position.x + 0.2f)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        else if (transform.position.x < target.position.x + 0.2f)
        {
            transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
        }

    }

    private void farPatten()
    {

        if (farTimer >= farTime)
        {
            if (farAttackCheck == false)
            {
                farPattenCheck = true;
            }

        }

        if (farPattenCheck)
        {
            nav.speed = 0;
            anim.SetFloat("AttackState", 0);
            anim.SetFloat("NormalState", 0);
            anim.SetTrigger("Attack");
            farPattenCheck = false;
            farAttackCheck = true;
        }



        if (farAttackStop)
        {

            farAttackStopTimer += Time.deltaTime;
            if (farAttackStopTimer >= 0.5f)
            {
                anim.speed = 1;
                box.enabled = true;
                transform.parent.position += targetVec.normalized * 20 * Time.deltaTime;
                if (farAttackStopTimer >= 0.8f)
                {
                    box.enabled = false;
                    bullet.Clear();
                    StartCoroutine(farPattenEndPosPatten());
                    farAttackStop = false;
                    farAttackCheck = false;
                    nav.speed = moveSpeed;
                    farAttackStopTimer = 0;
                    farTimer = 0;
                }
            }
        }


        if (farStartCheck)
        {
            noVicinityAttackTimer += Time.deltaTime;
            if (noVicinityAttackTimer >= 1.5f)
            {
                farStartCheck = false;
                noVicinityAttackTimer = 0;
            }
        }
    }


    IEnumerator farPattenEndPosPatten()
    {
        yield return null;
        float y = 0;

        for (int i = 0; i < 12; i++)
        {

            bullet.Add(PoolingManager.Instance.CreateObject("BlueBullet", GameManager.instance.GetEnemyAttackObjectPatten));
            bullet[i].transform.position = transform.parent.position;
            bullet[i].GetComponent<BulletMove>().Boss = this;
            bullet[i].transform.rotation = Quaternion.Euler(new Vector3(0, y, 0));
            y += 30;
        }

    }

    private void vicinityPatten()
    {
        if (farStartCheck)
        {
            return;
        }

        if (vicinityPattenCheck)
        {
            nav.speed = 0;

            attackRange = PoolingManager.Instance.CreateObject("BossEndAttackRange", transform);
            attackRange.transform.position = transform.parent.position;
            attackRange.GetComponent<MeleeAttackRange>().Boss = this;

            anim.SetFloat("AttackSpeed", 0.4f);
            anim.SetFloat("AttackState", 1);
            anim.SetFloat("SkillState", 0.5f);
            anim.SetTrigger("Attack");
            player.Pull(transform.parent.position, 1f, 4);

            vicinityCheck = true;
            vicinityPattenCheck = false;
        }
        else
        {

            if (vicinityCoolCheck)
            {
                nav.speed = moveSpeed;
                vicinityTimer += Time.deltaTime;
                if (vicinityTimer >= vicinityCoolTime)
                {
                    vicinityCoolCheck = false;
                    vicinityCheck = false;
                    vicinityTimer = 0;
                }
            }
        }
        if (attackRange != null)
        {
            attackRange.GetComponent<MeleeAttackRange>().SetAttack(vicinityAttackRangeCheck, vicinityAttack);
        }

    }


    private void HaxagonPatten()
    {
        if (halfPattenCheck)
        {
            if (halfPattenIng)
            {
                halfTimer += Time.deltaTime;
                if (halfTimer > 5)
                {
                    int halfNum = Random.Range(0, 10);
                    if (halfNum > 6)
                    {
                        StartCoroutine(HaxagonLaser());
                    }
                    halfPattenIng = false;
                }
            }
        }
        else
        {
            if (stat.HP <= stat.MAXHP / 2)
            {
                StartCoroutine(HaxagonLaser());
                halfPattenCheck = true;
            }
        }
    }

    IEnumerator HaxagonLaser()
    {
        for (int i = 0; i < 5; i++)
        {
            PoolingManager.Instance.CreateObject("HaxagonLaser", transform.parent.parent);
            yield return new WaitForSeconds(5);
            if (i == 5)
            {
                halfPattenIng = true;
            }
        }

    }

    private void enemyDie()
    {
        if (stat.HP <= 0)
        {
            deathCheck = true;

            anim.SetTrigger("Die");
            PoolingManager.Instance.RemoveAllPoolingObject(GameManager.instance.GetEnemyAttackObjectPatten.gameObject);
            SoundManager.instance.bgSoundPause();
            //보스죽었을때 어떻게할지 생각해보고 여기다작성
            Debug.Log("끗");
        }
    }


    //애니메이션용
    //멀리있을떄 돌진용
    private void FarStartAnim()
    {
        farStartCheck = true;
    }

    private void FarStopAnim()
    {
        targetVec = target.position - transform.position;
        anim.speed = 0;
        farAttackStop = true;
    }
    //근접
    private void VicinityStartAnim()
    {
        vicinityAttackRangeCheck = true;

    }

    private void VicinityAnim()
    {
        vicinityAttack = true;
        anim.SetFloat("AttackSpeed", 1);
    }
    private void VicinityEndAnim()
    {

        vicinityAttackRangeCheck = false;
        vicinityAttack = false;
        vicinityCoolCheck = true;
    }

}
