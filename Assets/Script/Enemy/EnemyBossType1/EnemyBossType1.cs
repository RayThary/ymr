using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBossType1 : Unit
{
    private int pattenType;
    private BombingPatten bombing;

    private bool moveAnimation = true;

    [SerializeField] private float attackDelay = 3;//공격간의 시간
    private float attackDelayTimer = 0;

    private bool attackAnimationCheck = false;
    private bool animationCheck = false;

    private int bombingType;
    private int beforeBombingType = -1;

    [SerializeField] private float nextBombingTime = 0.2f;
    [SerializeField] private List<GameObject> pullObject = new List<GameObject>();

    private bool patten4AttackRangeCheck;//공격범위의 소환시간
    private bool patten4AttackStart;//공격이들어가는시간
    private GameObject attackRange;

    [SerializeField] private Transform attackTrs;//패턴3번의 망치가내려찍는위치
    private Vector3 patten3Pos;

    private float halfPattenTime = 1;
    private float halfPattenTimer = 0;

    private NavMeshAgent nav;
    private Animator animator;
    private BoxCollider box;

    private Transform playerTrs;//플레이어위치
    private Player player;
    private Vector3 beforePlayerTrs;

    private bool deathCheck = false;
    
    protected new void Start()
    {
        base.Start();
        nav = GetComponentInParent<NavMeshAgent>();
        box = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        bombing = GetComponent<BombingPatten>();
        playerTrs = GameManager.instance.GetPlayerTransform;
        player = GameManager.instance.GetPlayer;

        BossUI.Instance.StatBoss = stat;
        float hp = 100;

        if (GameManager.instance.GetStageNum == 1)
        {
            hp = 100;
            stat.SetHp(hp);
        }
        else if (GameManager.instance.GetStageNum == 2)
        {
            hp = 150;
            stat.SetHp(hp);
        }
        else
        {
            hp = 100;
            stat.SetHp(hp);
            Debug.LogError($"StageNumError , StageNum ={GameManager.instance.GetStageNum}");
        }
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
        enemyAttackMotion();
        enemyAttackPattern();
        enemyHalfPatten();
        enemyDie();
    }

    private void enemyMove()
    {
        if (transform.position.x + 0.2f <= playerTrs.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }
        else if (transform.position.x - 0.2 >= playerTrs.position.x)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }

        if (moveAnimation)
        {
            nav.enabled = true;
            animator.SetFloat("RunState", 0.5f);
            moveAnimation = false;
        }

        if (nav.enabled == true)
        {
            nav.SetDestination(playerTrs.position);
        }

    }

    private void enemyAttackMotion()
    {
        float dis = Vector3.Distance(transform.position, playerTrs.position);

        if (pattenType == 0 && attackAnimationCheck == false)
        {
            attackDelayTimer += Time.deltaTime;
            if (attackDelayTimer >= attackDelay)
            {
                attackAnimationCheck = true;
            }
        }

        if (attackAnimationCheck && animationCheck == false)
        {
            animationCheck = true;
            if (dis < 3)
            {

                nav.enabled = false;
                animator.SetTrigger("Attack");
                animator.SetFloat("AttackSpeed", 0.3f);
                animator.SetFloat("AttackState", 1);
                animator.SetFloat("SkillState", 0);

                pattenType = 4;

            }
            else
            {
                int patten = Random.Range(0, 2);
                if (patten == 0)
                {
                    animator.SetTrigger("Attack");
                    animator.SetFloat("AttackState", 0);
                    animator.SetFloat("NormalState", 0);
                    beforePlayerTrs = playerTrs.position;
                }
                else if (patten == 1)
                {
                    animator.SetTrigger("Attack");
                    animator.SetFloat("AttackState", 0);
                    animator.SetFloat("NormalState", 1);
                }
            }

        }


    }

    private void enemyAttackPattern()
    {
        if (pattenType == 1)
        {

            if (playerTrs.position.x >= 7 && playerTrs.position.x <= 22 && playerTrs.position.z >= 7 && playerTrs.position.z <= 22)
            {
                pullObject.Clear();
                Vector3 targetVecX;
                Vector3 targetVecZ;

                if (playerTrs.position.x > beforePlayerTrs.x)
                {
                    targetVecX = new Vector3(playerTrs.position.x + 4, playerTrs.position.y, playerTrs.position.z);
                }
                else
                {
                    targetVecX = new Vector3(playerTrs.position.x - 4, playerTrs.position.y, playerTrs.position.z);
                }

                if (playerTrs.position.z > beforePlayerTrs.z)
                {
                    targetVecZ = new Vector3(playerTrs.position.x, playerTrs.position.y, playerTrs.position.z + 4);
                }
                else
                {
                    targetVecZ = new Vector3(playerTrs.position.x, playerTrs.position.y, playerTrs.position.z - 4);
                }
                for (int i = 0; i < 2; i++)
                {
                    pullObject.Add(PoolingManager.Instance.CreateObject("PullObject", GameManager.instance.GetEnemyAttackObjectPatten));
                    if (i == 0)
                    {
                        pullObject[i].transform.position = targetVecX;
                    }
                    else
                    {
                        pullObject[i].transform.position = targetVecZ;
                    }
                    pullObject[i].GetComponent<PullObject>().RemoveObject(5);
                }

                #region
                //for (int i = 0; i < 4; i++)
                //{
                //    Vector3 playerVec = playerTrs.position;
                //    Vector3 targetVec;
                //    if (i == 0)
                //    {
                //        targetVec = new Vector3(playerVec.x + 4, playerVec.y, playerVec.z);

                //    }
                //    else if (i == 1)
                //    {
                //        targetVec = new Vector3(playerVec.x - 4, playerVec.y, playerVec.z);

                //    }
                //    else if (i == 2)
                //    {
                //        targetVec = new Vector3(playerVec.x, playerVec.y, playerVec.z + 4);

                //    }
                //    else
                //    {
                //        targetVec = new Vector3(playerVec.x, playerVec.y, playerVec.z - 4);

                //    }

                //    pullObject.Add(PoolingManager.Instance.CreateObject("PullObject", GameManager.instance.GetEnemyAttackObjectPatten));
                //    pullObject[i].transform.position = targetVec;
                //    pullObject[i].GetComponent<PullObject>().RemoveObject(5);

                //}
                #endregion
                pattenType = 0;
                attackAnimationCheck = false;
                animationCheck = false;
            }
            else
            {
                pattenType = Random.Range(2, 4);//이떄는 외부에있으므로 다른패턴을쓴다
            }



        }

        if (pattenType == 2)
        {
            StartCoroutine(bombingPatten());
            pattenType = 0;
            attackAnimationCheck = false;
            animationCheck = false;
        }

        if (pattenType == 3)
        {
            GameObject obj = PoolingManager.Instance.CreateObject("Type1Patten3", GameManager.instance.GetEnemyAttackObjectPatten);
            obj.transform.position = patten3Pos;
            obj.GetComponent<Type1Patten3>().SetShotStart(true);
            pattenType = 0;
            attackDelayTimer = 0;
            attackAnimationCheck = false;
            animationCheck = false;
            Debug.Log($"Patten3 ={patten3Pos}");

        }

        if (pattenType == 4)
        {
            attackRange = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BossEndAttackRange, GameManager.instance.GetEnemyAttackObjectPatten);
            attackRange.transform.position = transform.position;
            attackRange.GetComponent<MeleeAttackRange>().Boss = this;
            player.Pull(transform.parent.position, 1f, 3.2f);
            attackDelayTimer = 0;
            pattenType = 0;
        }

        if (attackRange != null)
        {
            attackRange.GetComponent<MeleeAttackRange>().SetAttack(patten4AttackRangeCheck, patten4AttackStart);
        }

    }

    IEnumerator bombingPatten()
    {

        for (int i = 0; i < 4; i++)
        {
            bombingType = Random.Range(0, 4);
            while (bombingType == beforeBombingType)
            {
                bombingType = Random.Range(0, 4);
            }
            beforeBombingType = bombingType;

            if (beforeBombingType == 0)
            {
                bombing.BombingStart(BombingPatten.BombingType.Horizontal);
            }
            else if (beforeBombingType == 1)
            {
                bombing.BombingStart(BombingPatten.BombingType.Vertical);
            }
            else if (beforeBombingType == 2)
            {
                bombing.BombingStart(BombingPatten.BombingType.RightDiagonal);
            }
            else if (beforeBombingType == 3)
            {
                bombing.BombingStart(BombingPatten.BombingType.LeftDiagonal);
            }

            yield return new WaitForSeconds(nextBombingTime);
        }
    }

    private void enemyHalfPatten()
    {
        if (stat.HP <= stat.MAXHP / 2)
        {
            halfPattenTimer += Time.deltaTime;
            if (halfPattenTimer >= halfPattenTime)
            {
                GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.CurveBulletPatten, GameManager.instance.GetEnemyAttackObjectPatten);
                obj.GetComponent<Type1HalfPatten>().SetHalfPatten();
                halfPattenTime = Random.Range(2, 5);
                halfPattenTimer = 0;
            }
        }
    }

    private void enemyDie()
    {
        if (stat.HP <= 0)
        {
            deathCheck = true;

            animator.SetTrigger("Die");

        }
    }

    //애니메이터외부
    private void Patten1And2Animation()
    {
        pattenType = Random.Range(1, 4);
        attackDelayTimer = 0;
    }

    private void MeleeAttackStart()
    {
        patten4AttackRangeCheck = true;

    }

    private void MeleeAttackSpeed()
    {
        animator.SetFloat("AttackSpeed", 1);
        patten4AttackStart = true;
    }

    private void MeleeAttackEnd()
    {
        patten4AttackStart = false;
        patten4AttackRangeCheck = false;

        moveAnimation = true;
        attackAnimationCheck = false;
        animationCheck = false;
    }

    private void Patten3PostionCheck()
    {
        patten3Pos = attackTrs.position;
        patten3Pos.y = 0.1f;
        Debug.Log($"animation ={patten3Pos}");
        pattenType = 3;
    }

}
