using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlameMachine : Pet
{
    //���� ��Ÿ�
    [SerializeField]
    private float _range;
    //�������� (�ʹ� ������ ���̻� �ٰ����� �ʵ���)
    private bool _move;
    //����ó�� ���̰� ���� ������Ʈ
    private GameObject _flame;
    private float _angle;
    private float _radius;

    // Start is called before the first frame update
    void Start()
    {
        _move = true;
        _flame = transform.GetChild(1).gameObject;
        _angle = _flame.transform.GetChild(0).GetComponent<Image>().fillAmount * 360;
        _radius = _flame.transform.GetChild(0).GetComponent<Image>().rectTransform.rect.width * 0.5f;
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //���� ã�Ҵٸ� ���� ã�ư��� �׷��� �ʴٸ� ������ ã�ư���
    public override void Follow()
    {
        if (_target == null)
        {
            base.Follow();
            transform.eulerAngles += new Vector3(0, Rotate(10, _master.transform) * 0.1f, 0);
        }
        else
        {
            if(_move)
            {
                transform.position += (_target.transform.position - transform.position).normalized * Time.deltaTime * _speed;
            }
            transform.eulerAngles += new Vector3(0, Rotate(10, _target.transform) * 0.1f, 0);
        }
    }
    //����
    public override void Action()
    {
        //���� ã������ �ʹ� ������ �������� �ʵ���
        if(_target != null)
        {
            //�����ֱ� ���ƿ�
            if (_timer >= _cycle)
            {
                //�Ÿ��� �����
                if (Vector3.Distance(transform.position, _target.transform.position) < _range)
                {
                    //����
                    _timer = 0;
                    if (!_flame.activeSelf)
                        _flame.SetActive(true);
                    Collider[] colliders = FindTarget();
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        colliders[i].GetComponent<Unit>()?.Hit(_master, _value);
                    }
                }
                else
                {
                    //�־ ���� ���� ����
                    if (_flame.activeSelf)
                        _flame.SetActive(false);
                }
            }

            if (Vector3.Distance(transform.position, _target.transform.position) < 1f)
            {
                _move = false;
            }
            else
            {
                _move = true;
            }
        }
    }

    //ȸ�� ���� ��������
    private float Rotate(float max, Transform target)
    {
        if (target == null)
            return 0;

        Vector3 dir = (target.position - transform.position).normalized;
        // ����(dot)�� ���� ������ ����. (Acos�� ���� ������ ������ �� ���� ����)
        float dot = Vector3.Dot(transform.forward, dir);
        if (dot < 1.0f)
        {
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            // ������ ���� ������ ������ �Ǻ�.
            Vector3 cross = Vector3.Cross(transform.forward, dir);
            // ���� ��� ���� ���� ���� �ݿ�
            if (cross.y < 0)
            {
                angle = transform.rotation.eulerAngles.z - Mathf.Min(max, angle);
            }
            else
            {
                angle = transform.rotation.eulerAngles.z + Mathf.Min(max, angle);
            }
            return angle;
        }
        return 0;
    }

    private Collider[] FindTarget()
    {
        List<Collider> colliders = new ();
        colliders.AddRange(Physics.OverlapSphere(transform.position, _radius, LayerMask.GetMask("Enemy")));
        for (int i = 0; i < colliders.Count; i++)
        {
            if (!(GetAngle(colliders[i].transform) < _angle))
            {
                colliders.RemoveAt(i);
            }
        }
        Debug.Log(colliders.Count);
        return colliders.ToArray    ();
    }
    private float GetAngle(Transform target)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dir);
        return Mathf.Acos(dot) * Mathf.Rad2Deg;
    }
}
