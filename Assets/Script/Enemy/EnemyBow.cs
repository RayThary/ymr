using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBow : Unit
{
    [SerializeField] private float enemySpeed = 3f;

    [SerializeField] private Vector3 EnemyRecognitionBox;//적의 플레이어인식범위
    private Transform m_playerTrs;//플레이어
    private Animator m_anim;

    [SerializeField] private bool enemyInCheck;//사정거리안인지 체크하는부분
    [SerializeField] private bool enemyOutCheck;
    private bool enemyAttackAnim;
    private bool enemyAttackCheck = false;//공격체크
    private bool enemyNoAttack;//공격후 딜레이시간동안 공격안들어가게해주는부분
    private bool enemyAttackDelayCheck = true;//공격시간이됬는지체크하는부분
    [SerializeField] private float attackDelay = 2f;
    private float attackTimer = 0.0f;

    [SerializeField]
    private Launcher launcher;
    public Transform hand;
    public Transform muzzle;
    public Transform objectParent;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, EnemyRecognitionBox);
        Gizmos.DrawWireCube(transform.position, EnemyRecognitionBox * 0.7f);

    }
    new void Start()
    {
        base.Start();
        m_anim = GetComponentInChildren<Animator>();
        m_playerTrs = GameManager.instance.GetPlayerTransform;
        launcher = new Launcher(this, hand, muzzle, 0, 1, objectParent);
    }

    void Update()
    {
        objectParent = GameManager.instance.GetComponent<Transform>();
        launcher.Direction_Calculation(m_playerTrs.position);
        enemyPlayerBox();
        enemyMoveX();
        enemyMoveZ();
        enemyAttack();
        enemyAttackMotion();
    }

    private void enemyPlayerBox()
    {

        Collider[] playerCheck = Physics.OverlapBox(transform.position, EnemyRecognitionBox / 2, Quaternion.identity, LayerMask.GetMask("Player"));
        Collider[] playerOutCheck = Physics.OverlapBox(transform.position, EnemyRecognitionBox * 0.7f / 2, Quaternion.identity, LayerMask.GetMask("Player"));
        if (playerCheck.Length > 0)
        {
            enemyInCheck = true;
        }
        else
        {
            enemyInCheck = false;
        }

        if (playerOutCheck.Length > 0)
        {
            enemyOutCheck = true;
        }
        else
        {
            enemyOutCheck = false;
        }

    }

    private void enemyMoveX()
    {
        if (enemyInCheck == false)
        {

            if (transform.position.x > m_playerTrs.position.x)
            {
                transform.Translate(Vector3.left * enemySpeed * Time.deltaTime);
            }
            else if (transform.position.x < m_playerTrs.position.x)
            {
                transform.Translate(Vector3.right * enemySpeed * Time.deltaTime);
            }
        }
        else if (enemyInCheck == true)
        {
            if (enemyOutCheck == true) 
            {

                if (transform.position.x  < m_playerTrs.position.x )
                {
                    transform.Translate(Vector3.left * enemySpeed * Time.deltaTime);
                }
                else if (transform.position.x > m_playerTrs.position.x)
                {
                    transform.Translate(Vector3.right * enemySpeed * Time.deltaTime);
                }
            }
        }

    }
    private void enemyMoveZ()
    {
        if (enemyInCheck == false)
        {

            if (transform.position.z > m_playerTrs.position.z)
            {
                transform.Translate(Vector3.down * enemySpeed * Time.deltaTime);
            }
            else if (transform.position.z < m_playerTrs.position.z)
            {
                transform.Translate(Vector3.up * enemySpeed * Time.deltaTime);
            }
        }
        else if (enemyInCheck == true)
        {

            if (enemyOutCheck == true) 
            {

                {
                    if (transform.position.z < m_playerTrs.position.z )
                    {
                        transform.Translate(Vector3.down * enemySpeed * Time.deltaTime);
                    }

                    else if (transform.position.z > m_playerTrs.position.z)
                    {
                        transform.Translate(Vector3.up * enemySpeed * Time.deltaTime);
                    }

                }
            }

        }
    }


    private void enemyAttack()
    {
        if (enemyAttackDelayCheck == false)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelay)
            {
                enemyAttackDelayCheck = true;
                enemyNoAttack = false;
                attackTimer = 0.0f;
            }
            return;
        }

        if (enemyInCheck == true)
        {
            enemyAttackCheck = true;

        }
    }

    private void enemyAttackMotion()
    {
        if (enemyNoAttack)
        {
            return;
        }
        if (enemyAttackCheck)
        {
            enemyAttackAnim = true;
            m_anim.SetFloat("AttackState", 0);
            m_anim.SetFloat("NormalState", 0.5f);
            m_anim.SetTrigger("Attack");
            enemyAttackCheck = false;
            enemyNoAttack = true;
        }
    }


    //애니메이션용 
    private bool EnemyAttackStartCheck()
    {
        return enemyAttackDelayCheck = false;
    }

    private void EnemyBulletShoot()
    {
        launcher.Fire();
    }

    //자식참조용&외부참조용
    public Vector3 GetEnemyRecognitionBox()
    {
        return EnemyRecognitionBox;
    }
    public bool GetEnemyBulletShootCheck()
    {
        return enemyAttackAnim;
    }
    public void SetEnemyBulletShootCheck(bool _value)
    {
        enemyAttackAnim = _value;
    }
}
