using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlameMachine : Pet
{
    //공격 사거리
    [SerializeField]
    private float _range;
    //움직일지 (너무 가까우면 더이상 다가가지 않도록)
    private bool _move;
    //공격처럼 보이게 해줄 오브젝트
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
                    //멀어서 공격 하지 않음
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
