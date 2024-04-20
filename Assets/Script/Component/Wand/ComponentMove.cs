using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMove : IComponentObject
{
    private ComponentObject _componentFlame;
    private Unit _enemy;
    private float _radius = 1.5f;
    private float _speed = 2;


    public void Enter(Collider other)
    {

    }

    public void Fire(ComponentObject flame)
    {
        //주위에서 가장 가까운 적 발견
        Collider[] colliders = Physics.OverlapSphere(flame.transform.position, _radius, LayerMask.GetMask("Enemy"));
        if(colliders.Length == 0 )
            return;

        float MinDistance = Vector3.Distance(flame.transform.position, colliders[0].transform.position);
        int index = 0;
        for(int i = 1; i < colliders.Length; i++)
        {
            float Distance = Vector3.Distance(flame.transform.position, colliders[i].transform.position);
            if (Distance < MinDistance)
            {
                index = i;
                MinDistance = Distance;
            }
        }
        _enemy = colliders[index].GetComponent<Unit>();
        _componentFlame = flame;
    }

    public void Update()
    {
        //적을 따라가기
        if(_enemy != null )
        {
            Vector3 dir = (_enemy.transform.position - _componentFlame.transform.position).normalized;
            _componentFlame.transform.position += new Vector3(dir.x, 0, dir.z) * Time.deltaTime * _speed;
        }
    }

    public void Destroy()
    {

    }
}
