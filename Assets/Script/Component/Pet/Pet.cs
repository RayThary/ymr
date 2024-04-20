using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Pet : MonoBehaviour
{
    //주인
    [SerializeField]
    protected Player _master;
    public Player Master { get { return _master; } set { _master = value; } }
    //공격할 대상
    [SerializeField]
    protected Unit _target;

    //행동 주기
    [SerializeField]
    protected float _cycle;

    //그냥 행동의 시간을 재는 용도
    [SerializeField]
    protected float _timer;

    //이정도 거리 안에 적을 탐지함
    [SerializeField]
    protected float _senser = 10;

    [SerializeField]
    protected float _value;

    //따라가는 속도
    [SerializeField]
    protected float _speed = 2;

    //누군가를 따라갈지
    protected bool _follow = true;
    //무언가를 찾을지
    protected bool _find = true;
    //행동을 할지
    protected bool _action = true;

    // Update is called once per frame
    void Update()
    {
        //플레이어를 따라가기
        if(_follow)
            Follow();
        //주위 적을 찾기
        if (_find)
            Find();
        //행동
        if(_action)
            Action();

        _timer += Time.deltaTime;
    }

    //주인 찾아가기
    public virtual void Follow()
    {
        if(_master != null)
        {
            transform.position += (_master.transform.position - transform.position).normalized * Time.deltaTime * _speed;
        }
    }
    //적을 찾기
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
    //행동은 다 다르니까
    public abstract void Action();
}
