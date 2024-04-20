using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BombingPatten;

public class BombingObject : MonoBehaviour
{
    private int type;
    private LineRenderer line;
    private DangerZone_LineRenderer lineDangerZone;

    private float bombingTime = 1;//폭발시작까지의시간

    private Vector3 startPosVec;
    private Vector3 endPosVec;

    private Vector3 bombingStartPos;
    private bool startLeft = false;
    private float bombingIntervalTimer;

    private bool firstCheck = false;
    private bool lineFasleCheck = false;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        lineDangerZone = GetComponent<DangerZone_LineRenderer>();
        lineDangerZone.SetTime(bombingTime);
    }


    void Update()
    {
        lineShow();
        bombingShow();
    }

    private void lineShow()
    {
        if (firstCheck)
        {
            line.enabled = true;
            line.SetPosition(0, startPosVec);
            line.SetPosition(1, endPosVec);
            lineFasleCheck = true;
            firstCheck = false;
        }
    }

    private void bombingShow()
    {
        if (lineFasleCheck && line.enabled == false)
        {
            StartCoroutine(bombing());
            lineFasleCheck = false;
        }
    }

    IEnumerator bombing()
    {
        if (type==0)
        {
            for (int i = 0; i < 9; i++)
            {
                GameObject obj = null;
                
                if (startLeft)
                {
                    bombingStartPos += Vector3.right;
                }
                else
                {
                    bombingStartPos += Vector3.left;
                }
                obj = PoolingManager.Instance.CreateObject("BlueBombing", GameManager.instance.GetEnemyAttackObjectPatten);
                obj.transform.position = bombingStartPos;
                obj.GetComponent<Bombing>().SetSpawnCheck();
                yield return new WaitForSeconds(bombingIntervalTimer);
            }
        }
        else if (type == 1)
        {
            for (int i = 0; i < 9; i++)
            {
                GameObject obj = null;
                
                if (startLeft)
                {
                    bombingStartPos += Vector3.back;
                }
                else
                {
                    bombingStartPos += Vector3.forward;
                }
                obj = PoolingManager.Instance.CreateObject("BlueBombing", GameManager.instance.GetEnemyAttackObjectPatten);
                obj.transform.position = bombingStartPos;
                obj.GetComponent<Bombing>().SetSpawnCheck();
                yield return new WaitForSeconds(bombingIntervalTimer);
            }
        }
        else if (type == 2)
        {
            for (int i = 0; i < 9; i++)
            {
                GameObject obj = null;
                
                if (startLeft)
                {
                    bombingStartPos += new Vector3(+1, 0, -1);
                }
                else
                {
                    bombingStartPos += new Vector3(-1, 0, +1);
                }
                obj = PoolingManager.Instance.CreateObject("BlueBombing", GameManager.instance.GetEnemyAttackObjectPatten);
                obj.transform.position = bombingStartPos;
                obj.GetComponent<Bombing>().SetSpawnCheck();
                yield return new WaitForSeconds(bombingIntervalTimer);
            }
        }
        else if (type == 3)
        {
            for (int i = 0; i < 9; i++)
            {
                GameObject obj = null;
                
                if (startLeft)
                {
                    bombingStartPos += new Vector3(+1, 0, +1);
                }
                else
                {
                    bombingStartPos += new Vector3(-1, 0, -1);
                }
                obj = PoolingManager.Instance.CreateObject("BlueBombing", GameManager.instance.GetEnemyAttackObjectPatten);
                obj.transform.position = bombingStartPos;
                obj.GetComponent<Bombing>().SetSpawnCheck();

                yield return new WaitForSeconds(bombingIntervalTimer);
            }
        }
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }


    /// <summary>
    /// 오브젝트의 타입 그리고 시작지점과 끝지점 터지는시간 터질곳의 시작지점을 지정해주는곳
    /// </summary>
    /// <param name="_type">오브젝트의타입 부모의 이넘에서 인트형으로 형변환 해주어야함</param>
    /// <param name="_start">라인의 시작지점</param>
    /// <param name="_end">라인의 끝지점</param>
    /// <param name="_bombingTime">터지기전까지의 시간</param>
    /// <param name="_bombingStartPos">터지기 시작할위치</param>
    /// <param name="_startLeft">어느쪽부터 시작할지알려주는곳</param>
    public void SetObject(int _type, Vector3 _start, Vector3 _end, float _bombingTime, Vector3 _bombingStartPos, bool _startLeft,float _bombingIntervalTimer)
    {
        type = _type;
        startPosVec = _start;
        endPosVec = _end;
        bombingTime = _bombingTime;
        bombingStartPos = _bombingStartPos;
        startLeft = _startLeft;
        bombingIntervalTimer = _bombingIntervalTimer;
        firstCheck = true;
    }

}
