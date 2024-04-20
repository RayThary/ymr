using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullObject : MonoBehaviour
{
    private Player player;

    [SerializeField] private float pullHitTime = 2;// ���ʵ��� �־�� ��������Դ��������ִ°�
    private float pullTimer = 0;

    [SerializeField] private bool pullCheck = false;
    [SerializeField] private bool pullHitCheck = false;
    [SerializeField] private bool oneCheck = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.Pull(transform.position, 20, 3);
            oneCheck = true;
            pullCheck = true;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player.Pull(transform.position, 0, 0);
            pullCheck = false;
            pullHitCheck = false;
            pullTimer = 0;
        }
    }

    void Start()
    {
        player = GameManager.instance.GetPlayer;
    }

    void Update()
    {
        hitCheck();
        zeroPostionCheck();
        
    }

    private void hitCheck()
    {

        if (pullCheck)
        {
            pullTimer += Time.deltaTime;
            if (pullTimer > pullHitTime)
            {
                pullHitCheck = true;
            }
        }

        if (pullCheck && pullHitCheck)
        {
            player.Hit(null, 1);
            pullCheck = false;

        }

        if (!pullCheck && pullHitCheck)
        {
            pullTimer += Time.deltaTime;
            if (pullTimer >= pullHitTime * 2)
            {
                pullCheck = true;
                pullTimer = 0;
            }
        }

    }

    private void zeroPostionCheck()
    {
        if (pullCheck || pullHitCheck)
        {

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > 0.3f)
            {
                if (!oneCheck)
                {
                    player.Pull(transform.position, 20, 3);
                    oneCheck = true;
                }
            }
            else
            {
                player.Pull(transform.position, 0, 0);
                oneCheck = false;
            }
        }
    }

    /// <summary>
    /// ������Ʈ �������Ű�ºκ�
    /// </summary>
    /// <param name="_value">���ʵ��� ���ӽ�ų�������ִ°�</param>
    public void RemoveObject(float _value)
    {
        Invoke("removeObject", _value);
    }

    private void removeObject()
    {
        PoolingManager.Instance.RemovePoolingObject(gameObject);
        player.Pull(transform.position, 0, 0);
    }

}
