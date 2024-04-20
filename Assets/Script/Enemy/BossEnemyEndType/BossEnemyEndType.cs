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
    private Transform target;//�÷��̾�



    // �⺻���� 2��

    //��������
    private bool vicinityPattenCheck = false;//�����Ÿ��ȿ��ִ��� �ۿ��ִ��� üũ���ִºκ����θ������

    private bool vicinityCheck = false;//�ȿ������� �ѹ���üũ���ֱ����Ѱ�
    private bool vicinityCoolCheck = false;//��Ÿ���̰��ݳ��������ĺ��� �������ѺҰ�
    [SerializeField] private float vicinityCoolTime = 2;
    private float vicinityTimer = 0;

    private bool vicinityAttack = false;// �������� �����½ð� 
    private bool vicinityAttackRangeCheck = false; // ��ȯ�ǰ� ������½ð�
    private GameObject attackRange;

    //���� ����
    private bool farPattenCheck = false;
    private bool farAttackStop = false;//�ִϸ��̼��� ���������ֱ����ؼ�����
    private bool farAttackCheck = false;//������ �ð���üũ�����Ŀ� ���ݸ�ǽ����ϴºκ������ظ������

    private bool farStartCheck = false;

    private float farAttackStopTimer = 0;
    private Vector3 targetVec;
    private List<GameObject> bullet = new List<GameObject>();


    [SerializeField]private BoxCollider box;//������ �ǰ�������

    //���ʵ��� �긮�־�����üũ���ֱ����� ����
    [SerializeField] private float farTime = 5;
    private float farTimer = 0;


    private float noVicinityAttackTimer = 0;

    //�������� ����
    private bool halfPattenCheck = false;
    
    [SerializeField]private List<EndBossSeal> cube = new List<EndBossSeal>();


    [SerializeField] private float moveSpeed = 3.5f;

    private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("��Ʈ!");
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
    }


    void Update()
    {
        endBossMove();
        farPatten();
        vicinityPatten();
        HaxagonPatten();


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

        //�¿�üũ��
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
        if (halfPattenCheck == false)
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
        for (int i = 0; i < 99; i++)
        {
            PoolingManager.Instance.CreateObject("HaxagonLaser", transform.parent.parent);
            yield return new WaitForSeconds(5);

        }

    }

  


    //�ִϸ��̼ǿ�
    //�ָ������� ������
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
    //����
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
