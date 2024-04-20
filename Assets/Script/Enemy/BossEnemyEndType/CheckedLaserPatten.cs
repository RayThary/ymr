using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckedLaserPatten : MonoBehaviour
{
    /// <summary> #모양으로 레이저가 나오고 중앙기준으로 위아래 or 오른쪽왼쪽으로 움직이는패턴
    ///  이오브젝트는 리무브를 소환한객체에서 해주어야함
    /// </summary>
    private LineRenderer lineRen;
    //시작지점
    private Vector3 startPoint;
    private Vector3 midPoint;
    public Vector3 MidPoint
    {
        get => midPoint;
    }

    private Vector3 endPoint;

    //가로기준
    //0 의 z축은 시작지점 위치
    //1의 z 축은 끝지점 위치
    //세로면 x와 z를 바꾸면된다
    [SerializeField] private float moveSpeed = 1;

     BoxCollider box;//스테이지

    //위아래로 움직이면 true 좌우로움직이면 false
    [SerializeField] private bool UPAndDown;
    //오른쪽에서 시작하는라인이면 true 왼쪽시작이면 false
    [SerializeField] private bool Right;
    //위쪽에서 시작하는 라인이면 true 왼쪽시작이면 false
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

                //시작위치 백분률 시작부터 끝까지의 0~1로표현시 0.7부분의 위치에서시작 아래도똑같음
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

            //움직이는건 z축 근데 x축이 어디까지뻗어나가는지체크하기위한곳 중앙기준에서 뻗어나가는걸체크해줌
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
            //움직이는건 x축 근데 z축이 어디까지뻗어나가는지체크하기위한곳 중앙기준에서 뻗어나가는걸체크해줌
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
