using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindMillPatten : MonoBehaviour
{
    /// <summary> x자로 레이저를 발사하고 시계방향으로 도는 패턴
    /// 중앙기준으로 쏘는거라 벽에맞으면 막힘
    /// </summary>

    private LineRenderer lineRen;
    [SerializeField] BoxCollider box;//현재맵크기를알아오는곳 
    private Vector3 midPoint;//가운데위치 이부분부터 모든선이 나가는기준
    [SerializeField] private Transform startPoint;//나가는기준의 한쪽 오브젝트의 위치  
    [SerializeField] private Transform endPoint;// 나가는기준의 나머지한쪽 오브젝트 위치 
    [SerializeField] private float speed;//회전속도

    // 각각 오브젝트의 중앙으로부터 뻗어나갔을때 벽을다은위치를 받아줄곳
    private Vector3 startVec;
    private Vector3 endVec;
    //이오브젝트가 세로 시작이면 true 가로 시작이면 false
    [SerializeField] private bool Lenght;

    [SerializeField] private Unit boss;
    public Unit Boss { set => boss = value; }

    void Start()
    {
        box = GameManager.instance.GetStage;
        lineRen = GetComponent<LineRenderer>();
        midPoint = new Vector3(box.bounds.center.x, 0.1f, box.bounds.center.z);

        lineRen.SetPosition(1, midPoint);
        transform.position = midPoint;
        if (Lenght == true)
        {
            startPoint.position = new Vector3(midPoint.x, midPoint.y, midPoint.z + 5);
            endPoint.position = new Vector3(midPoint.x, midPoint.y, midPoint.z - 5);
        }
        else
        {
            startPoint.position = new Vector3(midPoint.x + 5, midPoint.y, midPoint.z);
            endPoint.position = new Vector3(midPoint.x - 5, midPoint.y, midPoint.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        laserMove();
        laserHit();
    }

    private void laserMove()
    {
        startPoint.RotateAround(midPoint, Vector3.up, speed * Time.deltaTime);
        endPoint.RotateAround(midPoint, Vector3.up, speed * Time.deltaTime);

        RaycastHit hit;

        if (Physics.Raycast(midPoint, startPoint.position - midPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Wall")))
        {
            startVec = hit.point;
            Debug.DrawLine(midPoint, startVec, Color.red);
        }
        if (Physics.Raycast(midPoint, endPoint.position - midPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Wall")))
        {
            endVec = hit.point;
            Debug.DrawLine(midPoint, endVec, Color.red);
        }

        lineRen.SetPosition(0, startVec);
        lineRen.SetPosition(2, endVec);
    }

    private void laserHit()
    {
        if (Physics.Linecast(startVec, endVec, LayerMask.GetMask("Player")))
        {
            Player player = GameManager.instance.GetPlayer;
            player.Hit(boss, 1);
        }
    }
}
