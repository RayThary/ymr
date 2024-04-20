using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindMillPatten : MonoBehaviour
{
    /// <summary> x�ڷ� �������� �߻��ϰ� �ð�������� ���� ����
    /// �߾ӱ������� ��°Ŷ� ���������� ����
    /// </summary>

    private LineRenderer lineRen;
    [SerializeField] BoxCollider box;//�����ũ�⸦�˾ƿ��°� 
    private Vector3 midPoint;//�����ġ �̺κк��� ��缱�� �����±���
    [SerializeField] private Transform startPoint;//�����±����� ���� ������Ʈ�� ��ġ  
    [SerializeField] private Transform endPoint;// �����±����� ���������� ������Ʈ ��ġ 
    [SerializeField] private float speed;//ȸ���ӵ�

    // ���� ������Ʈ�� �߾����κ��� ��������� ����������ġ�� �޾��ٰ�
    private Vector3 startVec;
    private Vector3 endVec;
    //�̿�����Ʈ�� ���� �����̸� true ���� �����̸� false
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
