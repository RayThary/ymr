using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UpGround : MonoBehaviour
{
    [SerializeField] private bool addLaserPatten = false;
    [SerializeField] private float laserShotTime = 2; // 오브젝트가 다올라온뒤 지정해준 시간뒤에 레이저를 발사한다
    private float laserShotTimer = 0.0f;
    private bool laserShotCheck = false;


    private bool upTimeCheck = false;
    private bool downTimeCheck = false;
    [SerializeField]private float timer = 0;
    [SerializeField] private float upSpeed = 1;
    private SpriteRenderer spr;
    [SerializeField]private float dangerZoneTime = 0;

    private bool stopWall = false;
    private float stopTime = 1.0f;
    private float stopTimer = 0.0f;

    private BoxCollider box;

    private Transform playerTrs;
    private Player player;



    //있다면 트루
    private bool horizontal;//수평 위치에 다른 함정이 있는지를 체크
    public bool Horizontal { set => horizontal = value; }
    private bool vertical;//수직 위치에 다른 함정이 있는지를 체크 
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
        //소환해줄위치를 소환자에서지정해줄것 y값만 -1.1로 해줘야함((-y*0.5)-0.1  값으로)
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

        //리무브
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
            transform.GetComponentInChildren<DangerZone>().setSprite(true);
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
        
        if (cubeWall)
        {
            if (horizontal == false)
            {
                if (transform.position.x > playerTrs.position.x)
                {
                    Debug.Log("왼쪽");
                    return new Vector3(-1, 0, 0);
                }
                else
                {
                    Debug.Log("오른쪽");
                    return new Vector3(1, 0, 0);
                }
            }
            else if (vertical == false)
            {
                if (transform.position.z > playerTrs.position.z)
                {
                    Debug.Log("아래쪽");
                    Debug.Log(transform.position.z);
                    Debug.Log(playerTrs.position.z);
                    return new Vector3(0, 0, -1);
                }
                else
                {
                    Debug.Log("위쪽");
                    return new Vector3(0, 0, 1);
                }
            }
            else
            {
                Debug.LogError("방향오류");
                return new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (horizontal == false)
            {
                if (transform.position.x > playerTrs.position.x)
                {
                    Debug.Log("오른쪽");
                    return new Vector3(1, 0, 0);
                }
                else
                {
                    Debug.Log("왼쪽");
                    return new Vector3(-1, 0, 0);
                }
            }
            else if (vertical == false)
            {
                if (transform.position.z > playerTrs.position.z)
                {
                    Debug.Log("위쪽");
                    return new Vector3(0, 0, 1);
                }
                else
                {
                    Debug.Log("아래쪽");
                    return new Vector3(0, 0, -1);
                }
            }
            else
            {
                Debug.LogError("방향오류");
                return new Vector3(0, 0, 0);
            }
        }



        // 엤날위치 보내주는곳
        #region
        //Vector3 hitdirection = transform.position - playerTrs.position;

        //bool right = playerTrs.position.x > transform.position.x;
        //bool up = playerTrs.position.z > transform.position.z;


        //bool isCloseX = hitdirection.x > hitdirection.z;

        //if (isCloseX && right)
        //{
        //    Debug.Log("오른쪽 방향");
        //    return new Vector3(1, 0, 0);
        //}
        //else if (isCloseX && right == false)
        //{
        //    Debug.Log("왼쪽 방향");
        //    return new Vector3(-1, 0, 0);
        //}
        //else if (isCloseX == false && up)
        //{
        //    Debug.Log("위 방향");
        //    return new Vector3(0, 0, 1);
        //}
        //else if (isCloseX == false && up == false)
        //{
        //    Debug.Log("아래 방향");
        //    return new Vector3(0, 0, -1);
        //}
        //else
        //{
        //    Debug.LogError("VectorError");//불값이이상하게들어감
        //    return new Vector3(0, 0, 0);
        //}

        #endregion


    }

}
