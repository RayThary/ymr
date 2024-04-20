using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombingPatten : MonoBehaviour
{
    public enum BombingType
    {
        Horizontal,
        Vertical,
        LeftDiagonal, //왼쪽위부터 오른쪽아래로 그어지는대각선
        RightDiagonal,//오른쪽위부터 왼쪽아래로 그어지는대각선
    }
    [SerializeField] private BombingType bType;
    [SerializeField] private bool bombingStartCheck = false;//field삭제예정 시작할지물어보는곳

    [SerializeField] private float bombingTime = 1;//폭발시작까지의시간
    [SerializeField] private float bombingIntervalTimer = 0.2f;//폭발간의시간간격

    private Transform centerTrs;

    private bool startLeft = false;

    private Vector3 startPosVec;
    private Vector3 centerPosVec;
    private Vector3 endPosVec;

    private Vector3 targetPos;//목표지점


    private bool bombingStart = false;



    void Start()
    {
        centerTrs = GameManager.instance.GetPlayerTransform;
    }


    void Update()
    {
        centerPostion();
    }

    private void centerPostion()
    {
        if (bombingStartCheck)
        {
            centerPosVec = centerTrs.position;
            int x = Mathf.RoundToInt(centerPosVec.x);
            int z = Mathf.RoundToInt(centerPosVec.z);

            centerPosVec = new Vector3(x, 0.1f, z);
            if (bType == BombingType.Horizontal)
            {
                startPosVec = new Vector3(centerPosVec.x - 4.5f, centerPosVec.y, centerPosVec.z);
                endPosVec = new Vector3(centerPosVec.x + 4.5f, centerPosVec.y, centerPosVec.z);

                if (startLeft)
                {
                    targetPos = startPosVec;
                    targetPos.x -= 0.5f;
                }
                else
                {
                    targetPos = endPosVec;
                    targetPos.x += 0.5f;
                }
            }
            else if (bType == BombingType.Vertical)
            {
                startPosVec = new Vector3(centerPosVec.x, centerPosVec.y, centerPosVec.z + 4.5f);
                endPosVec = new Vector3(centerPosVec.x, centerPosVec.y, centerPosVec.z - 4.5f);


                if (startLeft)
                {
                    //위에서 아래로
                    targetPos = startPosVec;
                    targetPos.z -= 0.5f;
                }
                else
                {
                    //아래에서 위로
                    targetPos = endPosVec;
                    targetPos.z += 0.5f;
                }
            }
            else if (bType == BombingType.LeftDiagonal)
            {
                startPosVec = new Vector3(centerPosVec.x - 4.5f, centerPosVec.y, centerPosVec.z + 4.5f);
                endPosVec = new Vector3(centerPosVec.x + 4.5f, centerPosVec.y, centerPosVec.z - 4.5f);

                if (startLeft)
                {
                    targetPos = startPosVec;
                    targetPos.x += 0.5f;
                    targetPos.z -= 0.5f;
                }
                else
                {
                    targetPos = endPosVec;
                    targetPos.x -= 0.5f;
                    targetPos.z += 0.5f;
                }
            }
            else if (bType == BombingType.RightDiagonal)
            {
                startPosVec = new Vector3(centerPosVec.x + 4.5f, centerPosVec.y, centerPosVec.z + 4.5f);
                endPosVec = new Vector3(centerPosVec.x - 4.5f, centerPosVec.y, centerPosVec.z - 4.5f);


                if (startLeft)
                {
                    targetPos = endPosVec;
                    targetPos.x += 0.5f;
                    targetPos.z += 0.5f;
                }
                else
                {
                    targetPos = startPosVec;
                    targetPos.x -= 0.5f;
                    targetPos.z -= 0.5f;
                }
            }
            GameObject obj = null;
            obj = PoolingManager.Instance.CreateObject("BombingObj", transform);
            BombingObject bomObj = obj.GetComponent<BombingObject>();
            bomObj.SetObject((int)bType, startPosVec, endPosVec, bombingTime, targetPos, startLeft, bombingIntervalTimer);
            bombingStartCheck = false;
        }

    }

 
    //폭발 위치와 소환되는코드
    #region
    /*
    IEnumerator bombing()
    {
        if (bType == BombingType.Horizontal)
        {
            for (int i = 0; i < 9; i++)
            {
                GameObject obj = null;
                if (startLeft)
                {
                    targetPos += Vector3.right;
                }
                else
                {
                    targetPos += Vector3.left;
                }
                obj = PoolingManager.Instance.CreateObject("BlueBombing", GameManager.instance.GetEnemyAttackObjectPatten);
                obj.transform.position = targetPos;
                yield return new WaitForSeconds(bombingIntervalTimer);
            }
        }
        else if (bType == BombingType.Vertical)
        {
            for (int i = 0; i < 9; i++)
            {
                GameObject obj = null;
                if (startLeft)
                {
                    targetPos += Vector3.back;
                }
                else
                {
                    targetPos += Vector3.forward;
                }
                obj = PoolingManager.Instance.CreateObject("BlueBombing", GameManager.instance.GetEnemyAttackObjectPatten);
                obj.transform.position = targetPos;
                yield return new WaitForSeconds(bombingIntervalTimer);
            }
        }
        else if (bType == BombingType.LeftDiagonal)
        {
            for (int i = 0; i < 9; i++)
            {
                GameObject obj = null;
                if (startLeft)
                {
                    targetPos += new Vector3(+1, 0, -1);
                }
                else
                {
                    targetPos += new Vector3(-1, 0, +1);
                }
                obj = PoolingManager.Instance.CreateObject("BlueBombing", GameManager.instance.GetEnemyAttackObjectPatten);
                obj.transform.position = targetPos;
                yield return new WaitForSeconds(bombingIntervalTimer);
            }
        }
        else if (bType == BombingType.RightDiagonal)
        {
            for (int i = 0; i < 9; i++)
            {
                GameObject obj = null;
                if (startLeft)
                {
                    targetPos += new Vector3(+1, 0, +1);
                }
                else
                {
                    targetPos += new Vector3(-1, 0, -1);
                }
                obj = PoolingManager.Instance.CreateObject("BlueBombing", GameManager.instance.GetEnemyAttackObjectPatten);
                obj.transform.position = targetPos;
                yield return new WaitForSeconds(bombingIntervalTimer);
            }
        }


    }*/
    #endregion

    public void BombingStart(BombingType _value)
    {
        bType = _value;
        bombingStartCheck = true;
    }
}
