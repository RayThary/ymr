using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BossType3 : Unit
{
    [SerializeField] private float AttackSpeed = 1;

    //기본공격 딜레이
    [SerializeField] private float basicAttackTime = 5f;
    private float basicAttackTimer = 0;

    //패턴의 딜레이
    [SerializeField] private float attackPattenTime = 5;
    private float attackPattenTimer = 0;
    private float beforeAttackPattenTime;
    private bool attackPattenStart = false;

    private bool patten0Anim = false;
    private bool patten1Anim = false;
    private bool patten2Anim = false;
    private bool patten3Anim = false;

    private int pattenNum = 0;


    [SerializeField] private float shieldChangeTime = 10;
    private float shieldChangeTimer = 0;
    private float beforeShieldChangeTime;

    [SerializeField] private GameObject objShield;

    private BoxCollider box;
    private Animator anim;
    private Transform playerTrs;
    private GroundPatten groundpatten;
    private ParticleSystem particle;

    private bool deathCheck = false;

    protected new void Start()
    {
        base.Start();
        AttackSpeed = 1;
        beforeAttackPattenTime = attackPattenTime;
        playerTrs = GameManager.instance.GetPlayerTransform;
        box = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        groundpatten = GetComponent<GroundPatten>();
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
        objShield.SetActive(false);


        beforeShieldChangeTime = shieldChangeTime;

        basicAttackTimer = basicAttackTime - 1;
        attackPattenTimer = attackPattenTime - 2;
        shieldChangeTimer = shieldChangeTime;

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
        basicAttackPatten();
        attackPatten();
        pattenAnimator();
        halfHpAddPatten();
        enemyDie();
    }


    private void basicAttackPatten()
    {
        basicAttackTimer += Time.deltaTime;
        if (basicAttackTimer >= basicAttackTime)
        {
            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.UpGroundLaserObj, GameManager.instance.GetEnemyAttackObjectPatten);
            obj.transform.position = playerTrs.position;
            UpGround upGround = obj.GetComponent<UpGround>();
            upGround.SetStopTime(true, 2);
            basicAttackTimer = 0;
        }
    }

    private void attackPatten()
    {
        if (attackPattenStart == false)
        {
            attackPattenTimer += Time.deltaTime;
            if (attackPattenTimer >= attackPattenTime)
            {
                attackPattenStart = true;

                int randomNum = Random.Range(0, 4);
                while (randomNum == pattenNum)
                {
                    randomNum = Random.Range(0, 4);
                }
                pattenNum = randomNum;
                #region
                //if (pattenNum == 0)
                //{
                //    pattenNum = Random.Range(0, 3);
                //    if (pattenNum == 0)
                //    {
                //        pattenNum = 1;
                //    }
                //    else if(pattenNum == 1)
                //    {
                //        pattenNum = 2;
                //    }
                //    else
                //    {
                //        pattenNum = 3;
                //    }
                //}
                //else if (pattenNum == 1)
                //{
                //    pattenNum = Random.Range(0, 3);
                //    if (pattenNum == 0)
                //    {
                //        pattenNum = 0;
                //    }
                //    else if (pattenNum == 1)
                //    {
                //        pattenNum = 2;
                //    }
                //    else
                //    {
                //        pattenNum = 3;
                //    }
                //}
                //else if(pattenNum == 2)
                //{
                //    pattenNum = Random.Range(0, 3);
                //    if (pattenNum == 0)
                //    {
                //        pattenNum = 0;
                //    }
                //    else if (pattenNum == 1)
                //    {
                //        pattenNum = 1;
                //    }
                //    else
                //    {
                //        pattenNum = 3;
                //    }
                //}
                //else if (pattenNum == 3)
                //{
                //    pattenNum = Random.Range(0, 3);
                //    pattenNum = Random.Range(0, 3);
                //    if (pattenNum == 0)
                //    {
                //        pattenNum = 0;
                //    }
                //    else if (pattenNum == 1)
                //    {
                //        pattenNum = 1;
                //    }
                //    else
                //    {
                //        pattenNum = 2;
                //    }

                //}
                #endregion

                int groundPattenBool = Random.Range(0, 2);


                if (pattenNum == 0)
                {
                    if (groundPattenBool == 0)
                    {
                        groundpatten.GroundPattenStart(GroundPatten.PattenName.HorizontalAndVerticalPatten, true);
                    }
                    else
                    {
                        groundpatten.GroundPattenStart(GroundPatten.PattenName.HorizontalAndVerticalPatten, false);
                    }
                    patten0Anim = true;
                    attackPattenTime = beforeAttackPattenTime;
                }
                else if (pattenNum == 1)
                {
                    if (groundPattenBool == 0)
                    {
                        groundpatten.GroundPattenStart(GroundPatten.PattenName.WavePattenHrizontal, true);
                    }
                    else
                    {
                        groundpatten.GroundPattenStart(GroundPatten.PattenName.WavePattenHrizontal, false);
                    }
                    attackPattenTime = attackPattenTime * 2;
                    patten1Anim = true;
                }
                else if (pattenNum == 2)
                {
                    if (groundPattenBool == 0)
                    {
                        groundpatten.GroundPattenStart(GroundPatten.PattenName.WavePattenVitical, true);
                    }
                    else
                    {
                        groundpatten.GroundPattenStart(GroundPatten.PattenName.WavePattenVitical, false);
                    }
                    attackPattenTime = attackPattenTime * 2;
                    patten2Anim = true;

                }
                else if (pattenNum == 3)
                {
                    groundpatten.GroundPattenStart(GroundPatten.PattenName.OpenWallGroundPatten);
                    patten2Anim = true;
                    attackPattenTime = beforeAttackPattenTime;
                }

                attackPattenTimer = 0;
            }
        }
    }

    private void pattenAnimator()
    {
        if (patten0Anim)
        {
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState", 0);
            anim.SetFloat("NormalState", 1);
            patten0Anim = false;
        }
        else if (patten1Anim || patten2Anim)
        {
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState", 0);
            anim.SetFloat("NormalState", 0);
            patten1Anim = false;
            patten2Anim = false;
        }
        else if (patten3Anim)
        {
            anim.SetTrigger("Attack");
            anim.SetFloat("AttackState", 1);
            anim.SetFloat("SkillState", 0);
            patten3Anim = false;
        }
    }

    private void halfHpAddPatten()
    {
        if (stat.HP > stat.MAXHP / 2)
        {
            return;
        }

        int shieldNum;
        ParticleSystem.MainModule main = particle.main;

        shieldChangeTimer += Time.deltaTime;
        if (shieldChangeTimer >= shieldChangeTime)
        {
            shieldNum = Random.Range(0, 10);
            if (shieldNum > 5)
            {
                main.startColor = Color.black;
                objShield.SetActive(true);
                shieldChangeTime = 5;
            }
            else
            {
                main.startColor = Color.white;
                objShield.SetActive(false);
                shieldChangeTime = beforeShieldChangeTime;
                basicAttackTime = basicAttackTime * 0.5f;
            }
            particle.Play();
            shieldChangeTimer = 0;
        }
    }

    private void enemyDie()
    {
        if (stat.HP <= 0)
        {
            deathCheck = true;

            anim.SetTrigger("Die");
            PoolingManager.Instance.RemoveAllPoolingObject(GameManager.instance.GetEnemyAttackObjectPatten.gameObject);

        }
    }

    //애니메이션용
    private void EnventEnd()
    {
        attackPattenStart = false;
    }

    private void PattenWaveEnvent()
    {
        anim.SetFloat("AttackSpeed", 0.1f);
    }
    private void PattenWaveEnventReturen()
    {
        anim.SetFloat("AttackSpeed", AttackSpeed);
    }

}
