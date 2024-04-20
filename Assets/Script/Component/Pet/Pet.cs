using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Pet : MonoBehaviour
{
    //����
    [SerializeField]
    protected Player _master;
    public Player Master { get { return _master; } set { _master = value; } }
    //������ ���
    [SerializeField]
    protected Unit _target;

    //�ൿ �ֱ�
    [SerializeField]
    protected float _cycle;

    //�׳� �ൿ�� �ð��� ��� �뵵
    [SerializeField]
    protected float _timer;

    //������ �Ÿ� �ȿ� ���� Ž����
    [SerializeField]
    protected float _senser = 10;

    [SerializeField]
    protected float _value;

    //���󰡴� �ӵ�
    [SerializeField]
    protected float _speed = 2;

    //�������� ������
    protected bool _follow = true;
    //���𰡸� ã����
    protected bool _find = true;
    //�ൿ�� ����
    protected bool _action = true;

    // Update is called once per frame
    void Update()
    {
        //�÷��̾ ���󰡱�
        if(_follow)
            Follow();
        //���� ���� ã��
        if (_find)
            Find();
        //�ൿ
        if(_action)
            Action();

        _timer += Time.deltaTime;
    }

    //���� ã�ư���
    public virtual void Follow()
    {
        if(_master != null)
        {
            transform.position += (_master.transform.position - transform.position).normalized * Time.deltaTime * _speed;
        }
    }
    //���� ã��
    public virtual void Find()
    {
        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, _senser, LayerMask.GetMask("Enemy")));
        Collider nearby = GameManager.instance.NearbyTrnasform(colliders.ToArray(), transform);
        while (colliders.Count > 0)
        {
            if (nearby.GetComponent<Unit>() != null)
            {
                break;
            }
            else
            {
                colliders.Remove(nearby);
                nearby = GameManager.instance.NearbyTrnasform(colliders.ToArray(), transform);
            }
        }
        if(nearby != null)
        {
            _target = nearby.GetComponent<Unit>();
        }
        else
        { _target = null; }
    }
    //�ൿ�� �� �ٸ��ϱ�
    public abstract void Action();
}
