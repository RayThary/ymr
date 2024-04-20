using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombingPatten : MonoBehaviour
{
    public enum BombingType
    {
        Horizontal,
        Vertical,
        LeftDiagonal, //���������� �����ʾƷ��� �׾����´밢��
        RightDiagonal,//������������ ���ʾƷ��� �׾����´밢��
    }
    [SerializeField] private BombingType bType;
    [SerializeField] private bool bombingStartCheck = false;//field�������� ������������°�

    [SerializeField] private float bombingTime = 1;//���߽��۱����ǽð�
    [SerializeField] private float bombingIntervalTimer = 0.2f;//���߰��ǽð�����

    private Transform centerTrs;

    private bool startLeft = false;

    private Vector3 startPosVec;
    private Vector3 centerPosVec;
    private Vector3 endPosVec;

    private Vector3 targetPos;//��ǥ����


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
                    //������ �Ʒ���
                    targetPos = startPosVec;
                    targetPos.z -= 0.5f;
                }
                else
                {
                    //�Ʒ����� ����
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

 
    //���� ��ġ�� ��ȯ�Ǵ��ڵ�
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
