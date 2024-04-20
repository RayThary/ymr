using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackRange : MonoBehaviour
{
    private SphereCollider sphere;

    [SerializeField]private bool attackStartCheck = false;//애니메이션의 시작시간
    private bool vicinityAttackCheck = false;//애니메이션의 공격중인시간

    [SerializeField]private bool attackAnimCheck = false;
    //반격없게할듯?
    private Player player;
    private Unit boss;
    public Unit Boss { set { boss = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (vicinityAttackCheck)
            {
                player.Hit(boss, 1);
            }
        }
    }


    void Start()
    {
        player = GameManager.instance.GetPlayer;
        sphere = GetComponent<SphereCollider>();
        sphere.enabled = false;
    }

    void Update()
    {
        if( attackStartCheck )
        {
            attackAnimCheck = true;
        }
        else
        {
            if (attackAnimCheck)
            {
                Invoke("attackStart", 0.05f);
            }
        }

        if (vicinityAttackCheck)
        {
            sphere.enabled = true;
        }
        else
        {
            sphere.enabled = false;
        }

    }
    private void attackStart()
    {
        PoolingManager.Instance.RemovePoolingObject(gameObject);
        attackAnimCheck = false;
    }

    /// <summary>
    /// 공격의 대미지를입히는시간과 소환물의시간을 지정해주는곳
    /// </summary>
    /// <param name="_startCheck">소환시작시 true 해주고 끝나고 false 해주는 소환시간을 지정해주는곳</param>
    /// <param name="_attackCheck">대미지를 입힐때만 true 해줄 bool값</param>
    public void SetAttack(bool _startCheck, bool _attackCheck)
    {
        attackStartCheck = _startCheck;
        vicinityAttackCheck = _attackCheck;
    }
}
