using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class FlameMachine : Pet
{
    //적을 찾았는지 표시하기
    //공격 사거리
    [SerializeField]
    private float _range;
    //움직일지 (너무 가까우면 더이상 다가가지 않도록)
    private bool _move;
    //공격처럼 보이게 해줄 오브젝트
    private GameObject _canvas;
    [SerializeField]
    private float _angle;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        _move = true;
        _canvas = transform.GetChild(1).gameObject;
        _angle = _canvas.transform.GetChild(0).GetComponent<Image>().fillAmount * 360;
        _senser = 8;
        animator = GetComponent<Animator>();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //적을 찾았다면 적을 찾아가고 그렇지 않다면 주인을 찾아가기
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
    //적을 찾기
    public override void Find()
    {
        //주인과 너무 멀어졌으니 적을 찾지 말고 주인에게 돌아가
        if(Vector3.Distance(transform.position, _master.transform.position) > _senser)
        {
            _target = null;
            return;
        }
        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(_master.transform.position, _senser, LayerMask.GetMask("Enemy")));
        Collider nearby = GameManager.instance.NearbyTrnasform(colliders.ToArray(), _master.transform);
        while (colliders.Count > 0)
        {
            if (nearby.GetComponent<Unit>() != null)
            {
                break;
            }
            else
            {
                colliders.Remove(nearby);
                nearby = GameManager.instance.NearbyTrnasform(colliders.ToArray(), _master.transform);
            }
        }
        if (nearby != null)
        {
            _target = nearby.GetComponent<Unit>();
        }
        else
        { _target = null; }
    }
    //공격
    public override void Action()
    {
        //적을 찾았을때 너무 가까우면 움직이지 않도록
        if(_target != null)
        {
            //공격주기 돌아옴
            if (_timer >= _cycle)
            {
                //거리가 가까움
                if (Vector3.Distance(transform.position, _target.transform.position) < _range)
                {
                    //공격
                    animator.SetBool("Attack", true);
                    _timer = 0;
                    if (!_canvas.activeSelf)
                        _canvas.SetActive(true);
                    Collider[] colliders = FindTarget();
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        colliders[i].GetComponent<Unit>()?.Hit(_master, _value);
                    }
                }
                else
                {
                    animator.SetBool("Attack", false);
                    //멀어서 공격 하지 않음
                    if (_canvas.activeSelf)
                    {
                        _canvas.SetActive(false);
                    }
                }
            }

            if (Vector3.Distance(transform.position, _target.transform.position) < 1.5f)
            {
                _move = false;
            }
            else
            {
                _move = true;
            }
        }
        else
        {
            animator.SetBool("Attack", true);
            if (_canvas.activeSelf)
            {
                _canvas.SetActive(false);
            }
        }
    }

    //회전 각도 리턴해줌
    private float Rotate(float max, Transform target)
    {
        if (target == null)
            return 0;

        Vector3 dir = (target.position - transform.position).normalized;
        // 내적(dot)을 통해 각도를 구함. (Acos로 나온 각도는 방향을 알 수가 없음)
        float dot = Vector3.Dot(transform.forward, dir);
        if (dot < 1.0f)
        {
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            // 외적을 통해 각도의 방향을 판별.
            Vector3 cross = Vector3.Cross(transform.forward, dir);
            // 외적 결과 값에 따라 각도 반영
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
        colliders.AddRange(Physics.OverlapSphere(transform.position, _range * 2, LayerMask.GetMask("Enemy")));
        for (int i = 0; i < colliders.Count; i++)
        {
            if (!(GetAngle(colliders[i].transform) < _angle))
            {
                colliders.RemoveAt(i);
            }
        }
        return colliders.ToArray();
    }
    private float GetAngle(Transform target)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dir);
        if (dot > 0.99f)
        {
            // 두 벡터가 거의 같은 방향을 가리킴
            return 0;
        }
        else
        {
            // 두 벡터가 다른 방향을 가리킴
            return Mathf.Acos(dot) * Mathf.Rad2Deg;
        }
    }
}
