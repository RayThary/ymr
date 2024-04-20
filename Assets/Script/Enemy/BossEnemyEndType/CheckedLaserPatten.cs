using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckedLaserPatten : MonoBehaviour
{
    /// <summary> #������� �������� ������ �߾ӱ������� ���Ʒ� or �����ʿ������� �����̴�����
    ///  �̿�����Ʈ�� �����긦 ��ȯ�Ѱ�ü���� ���־����
    /// </summary>
    private LineRenderer lineRen;
    //��������
    private Vector3 startPoint;
    private Vector3 midPoint;
    public Vector3 MidPoint
    {
        get => midPoint;
    }

    private Vector3 endPoint;

    //���α���
    //0 �� z���� �������� ��ġ
    //1�� z ���� ������ ��ġ
    //���θ� x�� z�� �ٲٸ�ȴ�
    [SerializeField] private float moveSpeed = 1;

     BoxCollider box;//��������

    //���Ʒ��� �����̸� true �¿�ο����̸� false
    [SerializeField] private bool UPAndDown;
    //�����ʿ��� �����ϴ¶����̸� true ���ʽ����̸� false
    [SerializeField] private bool Right;
    //���ʿ��� �����ϴ� �����̸� true ���ʽ����̸� false
    [SerializeField] private bool Up;

    [SerializeField] private bool IsTestDebuging = false;

    private float startX;
    private float endX;
    private float startXZPso;

    private bool moveReturn = true;

    void Start()
    {
        lineRen = GetComponent<LineRenderer>();
        box = GameManager.instance.GetStage;

        if (UPAndDown)
        {
            if (Up)
            {
                startXZPso = (box.bounds.max.z - box.bounds.min.z) * 0.75f;

                startPoint = new Vector3(0, 0.1f, box.bounds.min.z + startXZPso);
                midPoint = new Vector3(box.bounds.center.z, 0.1f, box.bounds.min.z + startXZPso);
                endPoint = new Vector3(box.bounds.center.z * 2, 0.1f, box.bounds.min.z + startXZPso);

                startX = Mathf.Lerp(box.bounds.min.z, box.bounds.max.z, 0.6f);
                endX = Mathf.Lerp(box.bounds.min.z, box.bounds.max.z, 0.9f);


                moveReturn = false;
            }
            else
            {
                startXZPso = (box.bounds.max.z - box.bounds.min.z) * 0.25f;

                startPoint = new Vector3(0, 0.1f, box.bounds.min.z + startXZPso);
                midPoint = new Vector3(box.bounds.center.z, 0.1f, box.bounds.min.z + startXZPso);
                endPoint = new Vector3(box.bounds.center.z * 2, 0.1f, box.bounds.min.z + startXZPso);

                startX = Mathf.Lerp(box.bounds.min.z, box.bounds.max.z, 0.1f);
                endX = Mathf.Lerp(box.bounds.min.z, box.bounds.max.z, 0.4f);

               

                moveReturn = true;
            }
        }
        else
        {
            if (Right)
            {

                //������ġ ��з� ���ۺ��� �������� 0~1��ǥ���� 0.7�κ��� ��ġ�������� �Ʒ����Ȱ���
                startXZPso = (box.bounds.max.x - box.bounds.min.x) * 0.75f;

                startPoint = new Vector3(box.bounds.min.x + startXZPso, 0.1f, 0);
                midPoint = new Vector3(box.bounds.min.x + startXZPso, 0.1f, box.bounds.center.z);
                endPoint = new Vector3(box.bounds.min.x + startXZPso, 0.1f, box.bounds.center.z * 2);

                startX = Mathf.Lerp(box.bounds.min.x, box.bounds.max.x, 0.6f);
                endX = Mathf.Lerp(box.bounds.min.x, box.bounds.max.x, 0.9f);

                moveReturn = false;
            }
            else
            {
                startXZPso = (box.bounds.max.x - box.bounds.min.x) * 0.25f;


                startPoint = new Vector3(box.bounds.min.x + startXZPso, 0.1f, 0);
                midPoint = new Vector3(box.bounds.min.x + startXZPso, 0.1f, box.bounds.center.z);
                endPoint = new Vector3(box.bounds.min.x + startXZPso, 0.1f, box.bounds.center.z * 2);

                startX = Mathf.Lerp(box.bounds.min.x, box.bounds.max.x, 0.1f);
                endX = Mathf.Lerp(box.bounds.min.x, box.bounds.max.x, 0.4f);

                moveReturn = true;
            }
        }


        lineRen.SetPosition(0, startPoint);
        lineRen.SetPosition(1, midPoint);
        lineRen.SetPosition(2, endPoint);


    }

    // Update is called once per frame
    void Update()
    {
        laserMove();
        playerHitCheck();
    }
    private void laserMove()
    {
        RaycastHit hit;
        if (UPAndDown)
        {

            //�����̴°� z�� �ٵ� x���� ���������������üũ�ϱ����Ѱ� �߾ӱ��ؿ��� ������°�üũ����
            if (Physics.Raycast(midPoint, Vector3.right, out hit, Mathf.Infinity, LayerMask.GetMask("Wall")))
            {
                startPoint.x = hit.point.x;
            }
            if (Physics.Raycast(midPoint, Vector3.left, out hit, Mathf.Infinity, LayerMask.GetMask("Wall")))
            {
                endPoint.x = hit.point.x;
            }

            if (startX >= midPoint.z)
            {
                moveReturn = true;
            }
            else if (endX <= midPoint.z)
            {
                moveReturn = false;
            }

            if (moveReturn)
            {
                startPoint.z += Time.deltaTime * moveSpeed;
                midPoint.z += Time.deltaTime * moveSpeed;
                endPoint.z += Time.deltaTime * moveSpeed;
            }
            else
            {
                startPoint.z -= Time.deltaTime * moveSpeed;
                midPoint.z -= Time.deltaTime * moveSpeed;
                endPoint.z -= Time.deltaTime * moveSpeed;
            }

            lineRen.SetPosition(0, startPoint);
            lineRen.SetPosition(1, midPoint);
            lineRen.SetPosition(2, endPoint);
        }
        else
        {
            //�����̴°� x�� �ٵ� z���� ���������������üũ�ϱ����Ѱ� �߾ӱ��ؿ��� ������°�üũ����
            if (Physics.Raycast(midPoint, Vector3.back, out hit, Mathf.Infinity, LayerMask.GetMask("Wall")))
            {
                startPoint.z = hit.point.z;
            }
            if (Physics.Raycast(midPoint, Vector3.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Wall")))
            {
                endPoint.z = hit.point.z;
            }



            if (startX >= midPoint.x)
            {
                moveReturn = true;
            }
            else if (endX <= midPoint.x)
            {
                moveReturn = false;
            }

            if (moveReturn)
            {
                startPoint.x += Time.deltaTime * moveSpeed;
                midPoint.x += Time.deltaTime * moveSpeed;
                endPoint.x += Time.deltaTime * moveSpeed;
            }
            else
            {
                startPoint.x -= Time.deltaTime * moveSpeed;
                midPoint.x -= Time.deltaTime * moveSpeed;
                endPoint.x -= Time.deltaTime * moveSpeed;
            }



            lineRen.SetPosition(0, startPoint);
            lineRen.SetPosition(1, midPoint);
            lineRen.SetPosition(2, endPoint);
        }
    }

    private void playerHitCheck()
    {
        if (Physics.Linecast(startPoint, endPoint, LayerMask.GetMask("Player")))
        {
            Player player = GameManager.instance.GetPlayer;
            player.Hit(null, 1);
        }
    }

}
