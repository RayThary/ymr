using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : Unit
{
    [SerializeField] private int pattenType; // 임시패턴설정용
    private BombingPatten bombing;

    [SerializeField] private float attackDelay = 3;
    private float attackDelayTimer = 0;

    private bool attackAnimationCheck = false;
    private bool animationCheck = false;

    private int bombingType;
    private int beforeBombingType = -1;

    [SerializeField] private float nextBombingTime = 0.2f;
    [SerializeField] private List<GameObject> pullObject = new List<GameObject>();

    private NavMeshAgent nav;
    private Rigidbody rigd;
    private Animator animator;

    private Transform playerTrs;//플레이어위치


    protected new void Start()
    {
        base.Start();
        nav = GetComponentInParent<NavMeshAgent>();
        rigd = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        bombing = GetComponent<BombingPatten>();
        playerTrs = GameManager.instance.GetPlayerTransform;

    }

    // Update is called once per frame
    void Update()
    {
        enemyMove();
        enemyAttackMotion();
        enemyAttackPattern();


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
        nav.SetDestination(playerTrs.position);
    }

    private void enemyAttackMotion()
    {
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
            int patten = Random.Range(0, 1);//패턴개수에따라 다르게설정

            if (patten == 0)
            {
                animator.SetTrigger("Attack");
                animator.SetFloat("AttackState", 0);
                animator.SetFloat("NormalState", 0);
            }
            else if (patten == 1)
            {
                Debug.Log("아직만들지않음");
            }
        }


    }

    private void enemyAttackPattern()
    {
        if (pattenType == 1)
        {
            if (pullObject.Count > 0)
            {
                if (pullObject[0].activeSelf == false)
                {
                    
                }
            }
            else
            {
                pullObject.Clear();
            }


            if (playerTrs.position.x >= 7 && playerTrs.position.x <= 22 && playerTrs.position.z >= 7 && playerTrs.position.z <= 22)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector3 playerVec = playerTrs.position;
                    Vector3 targetVec;
                    if (i == 0)
                    {
                        targetVec = new Vector3(playerVec.x + 4, playerVec.y, playerVec.z);

                    }
                    else if (i == 1)
                    {
                        targetVec = new Vector3(playerVec.x - 4, playerVec.y, playerVec.z);

                    }
                    else if (i == 2)
                    {
                        targetVec = new Vector3(playerVec.x, playerVec.y, playerVec.z + 4);

                    }
                    else
                    {
                        targetVec = new Vector3(playerVec.x, playerVec.y, playerVec.z - 4);

                    }

                    pullObject.Add(PoolingManager.Instance.CreateObject("PullObject", GameManager.instance.GetEnemyAttackObjectPatten));
                    pullObject[i].transform.position = targetVec;
                    pullObject[i].GetComponent<PullObject>().RemoveObject(5);

                }
                pattenType = 0;
                attackAnimationCheck = false;
                animationCheck = false;
            }
            else
            {
                pattenType = 2;//이떄는 외부에있으므로 다른패턴을쓴다
            }



        }


        if (pattenType == 2)
        {
            StartCoroutine(bombingPatten());
            pattenType = 0;
            attackAnimationCheck = false;
            animationCheck = false;
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

    //애니메이터외부
    private void Patten1And2Animation()
    {
        pattenType = Random.Range(1, 3);
        attackDelayTimer = 0;
    }


}
