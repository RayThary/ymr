using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMine : IComponentObject
{
    private float _damage = 2;
    private float _radius = 1.25f;
    private ComponentObject _parent;

    public void Update()
    {

    }

    public void Enter(Collider other)
    {
        if (other.GetComponent<Unit>() != null)
        {
            Mine mine = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.Mine, null).GetComponent<Mine>();
            mine.transform.position = _parent.transform.position;
            mine.Spawn(_parent.Player, _damage, _radius);
        }
    }

    public void Fire(ComponentObject bullet)
    {
        _parent = bullet;
    }
}


