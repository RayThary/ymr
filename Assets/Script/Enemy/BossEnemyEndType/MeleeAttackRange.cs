using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackRange : MonoBehaviour
{
    private SphereCollider sphere;

    [SerializeField]private bool attackStartCheck = false;//�ִϸ��̼��� ���۽ð�
    private bool vicinityAttackCheck = false;//�ִϸ��̼��� �������νð�

    [SerializeField]private bool attackAnimCheck = false;
    //�ݰݾ����ҵ�?
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
    /// ������ ������������½ð��� ��ȯ���ǽð��� �������ִ°�
    /// </summary>
    /// <param name="_startCheck">��ȯ���۽� true ���ְ� ������ false ���ִ� ��ȯ�ð��� �������ִ°�</param>
    /// <param name="_attackCheck">������� �������� true ���� bool��</param>
    public void SetAttack(bool _startCheck, bool _attackCheck)
    {
        attackStartCheck = _startCheck;
        vicinityAttackCheck = _attackCheck;
    }
}
