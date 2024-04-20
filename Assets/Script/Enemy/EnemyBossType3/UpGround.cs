using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UpGround : MonoBehaviour
{
    [SerializeField] private bool addLaserPatten = false;
    [SerializeField] private float laserShotTime = 2; // ������Ʈ�� �ٿö�µ� �������� �ð��ڿ� �������� �߻��Ѵ�
    private float laserShotTimer = 0.0f;
    private bool laserShotCheck = false;


    private bool upTimeCheck = false;
    private bool downTimeCheck = false;
    private float timer = 0;
    [SerializeField] private float upSpeed = 1;
    private SpriteRenderer spr;
    private float dangerZoneTime = 0;

    private bool stopWall = false;
    private float stopTime = 1.0f;
    private float stopTimer = 0.0f;

    private BoxCollider box;

    private Transform playerTrs;
    private Player player;



    //�ִٸ� Ʈ��
    private bool horizontal;//���� ��ġ�� �ٸ� ������ �ִ����� üũ
    public bool Horizontal { set => horizontal = value; }
    private bool vertical;//���� ��ġ�� �ٸ� ������ �ִ����� üũ 
    public bool Vertical { set => vertical = value; }

    private bool cubeWall;
    public bool CubeWall { set => cubeWall = value; }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player.Hit(null, 2);
        }

    }
    void Start()
    {
        //��ȯ������ġ�� ��ȯ�ڿ����������ٰ� y���� -1.1�� �������((-y*0.5)-0.1  ������)
        transform.position = new Vector3(transform.position.x, -1.1f, transform.position.z);

        spr = GetComponentInChildren<SpriteRenderer>();
        box = GetComponent<BoxCollider>();
        box.enabled = false;

        dangerZoneTime = gameObject.GetComponentInChildren<DangerZone>().getTime();
        dangerZoneTime += 0.3f;

        playerTrs = GameManager.instance.GetPlayerTransform;
        player = playerTrs.GetComponent<Player>();
    }


    void Update()
    {
        blockUpTime();
        blockUp();

    }

    private void blockUpTime()
    {
        if (downTimeCheck)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= dangerZoneTime)
        {
            upTimeCheck = true;
        }
    }

    private void blockUp()
    {
        if (upTimeCheck)
        {
            transform.position += new Vector3(0, upSpeed * Time.deltaTime, 0);
            box.enabled = true;
        }

        if (transform.position.y >= 0.5f)
        {
            upTimeCheck = false;
            downTimeCheck = true;
        }

        if (downTimeCheck)
        {
            if (stopWall)
            {
                if (addLaserPatten)
                {
                    laserShotTimer += Time.deltaTime;
                    if (laserShotTimer >= laserShotTime && laserShotCheck == false)
                    {
                        GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.LaserPatten, GameManager.instance.GetEnemyAttackObjectPatten);
                        obj.transform.position = transform.position;
                        laserShotCheck = true;
                    }

                    if (laserShotCheck == true)
                    {
                        stopTimer += Time.deltaTime;
                        if (stopTimer >= stopTime)
                        {
                            transform.position += new Vector3(0, -upSpeed * Time.deltaTime, 0);
                            if (transform.position.y <= -1)
                            {
                                box.enabled = false;
                            }
                        }
                    }
                }
                else
                {
                    stopTimer += Time.deltaTime;
                    if (stopTimer >= stopTime)
                    {
                        transform.position += new Vector3(0, -upSpeed * Time.deltaTime, 0);
                        if (transform.position.y <= -1)
                        {
                            box.enabled = false;
                        }
                    }
                }
            }
            else
            {
                transform.position += new Vector3(0, -upSpeed * Time.deltaTime, 0);
                if (transform.position.y <= -1)
                {
                    box.enabled = false;
                }
            }
        }

        //������
        if (transform.position.y <= -1.3f)
        {

            downTimeCheck = false;
            upTimeCheck = false;
            timer = 0;
            laserShotTimer = 0;
            stopTimer = 0;
            stopWall = false;
            laserShotCheck = false;
            transform.position = new Vector3(transform.position.x, -1.1f, transform.position.z);
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }





    public void SetStopTime(bool _stopWall, float _stopTime)
    {
        stopWall = _stopWall;
        stopTime = _stopTime;
    }

    public Vector3 playerHitDirection()
    {
        //cubeWall �����ʿ� �ִºκ��� �׶��׶� ���Ë� �����ִ°ɷιٲٴ°������� �����ʿ�
        if (cubeWall)
        {
            if (horizontal == false)
            {
                if (transform.position.x > playerTrs.position.x)
                {
                    Debug.Log("����");
                    return new Vector3(-1, 0, 0);
                }
                else
                {
                    Debug.Log("������");
                    return new Vector3(1, 0, 0);
                }
            }
            else if (vertical == false)
            {
                if (transform.position.z > playerTrs.position.z)
                {
                    Debug.Log("�Ʒ���");
                    Debug.Log(transform.position.z);
                    Debug.Log(playerTrs.position.z);
                    return new Vector3(0, 0, -1);
                }
                else
                {
                    Debug.Log("����");
                    return new Vector3(0, 0, 1);
                }
            }
            else
            {
                Debug.LogError("�������");
                return new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (horizontal == false)
            {
                if (transform.position.x > playerTrs.position.x)
                {
                    Debug.Log("������");
                    return new Vector3(1, 0, 0);
                }
                else
                {
                    Debug.Log("����");
                    return new Vector3(-1, 0, 0);
                }
            }
            else if (vertical == false)
            {
                if (transform.position.z > playerTrs.position.z)
                {
                    Debug.Log("����");
                    return new Vector3(0, 0, 1);
                }
                else
                {
                    Debug.Log("�Ʒ���");
                    return new Vector3(0, 0, -1);
                }
            }
            else
            {
                Debug.LogError("�������");
                return new Vector3(0, 0, 0);
            }
        }



        // �x����ġ �����ִ°�
        #region
        //Vector3 hitdirection = transform.position - playerTrs.position;

        //bool right = playerTrs.position.x > transform.position.x;
        //bool up = playerTrs.position.z > transform.position.z;


        //bool isCloseX = hitdirection.x > hitdirection.z;

        //if (isCloseX && right)
        //{
        //    Debug.Log("������ ����");
        //    return new Vector3(1, 0, 0);
        //}
        //else if (isCloseX && right == false)
        //{
        //    Debug.Log("���� ����");
        //    return new Vector3(-1, 0, 0);
        //}
        //else if (isCloseX == false && up)
        //{
        //    Debug.Log("�� ����");
        //    return new Vector3(0, 0, 1);
        //}
        //else if (isCloseX == false && up == false)
        //{
        //    Debug.Log("�Ʒ� ����");
        //    return new Vector3(0, 0, -1);
        //}
        //else
        //{
        //    Debug.LogError("VectorError");//�Ұ����̻��ϰԵ�
        //    return new Vector3(0, 0, 0);
        //}

        #endregion


    }
}
