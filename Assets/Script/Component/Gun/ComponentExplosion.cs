using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentExplosion : IComponentObject
{
    private ComponentObject _bullet;
    private float _radius = 1.25f;
    private float _damage = 1;

    private void Explosion()
    {
        Explosion _explosion;
        _explosion = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.Explosion, null).GetComponent<Explosion>();
        _explosion.transform.position = _bullet.transform.position;
        _explosion.Operation(_radius, _damage, _bullet.Player);
    }

    public void Fire(ComponentObject bullet)
    {
        _bullet = bullet;
    }

    public void Update()
    {

    }

    public void Enter(Collider other)
    {
        Explosion();
    }
}
